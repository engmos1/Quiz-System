using AutoMapper;
using BuisnessModel.DTOs.Question;
using BuisnessModel.Services;
using BuisnessModel.Services.JobsService;
using BuisnessModel.VeiwModels.Question;
using DataAccess.Models.Enums;
using ExaminationSystem.Attributes;
using ExaminationSystem.ViewModels;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Quiz_System.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly QuestionService _service;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public QuestionController(QuestionService service, IMapper mapper, IDistributedCache cache)
        {
            _service = service;
            _mapper = mapper;
            _cache = cache;
        }

        [HttpGet]
        [Benchmark]
        public async Task<ResponseViewModel<IEnumerable<AllQuestionsViewModel>>> GetAll()
        {
            var cacheKey = "questions_all";
            var cached = await _cache.GetStringAsync(cacheKey);

            IEnumerable<AllQuestionsDto> result;

            if (cached != null)
            {
                result = JsonSerializer.Deserialize<IEnumerable<AllQuestionsDto>>(cached);
            }
            else
            {
                result = await _service.GetAllQuestions();

                if (!result.Any())
                    return ResponseViewModel<IEnumerable<AllQuestionsViewModel>>
                        .Failure("No questions found.", ErrorCode.QuestionNotFound);

                await _cache.SetStringAsync(
                    cacheKey,
                    JsonSerializer.Serialize(result),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    }
                );
            }

            var vm = _mapper.Map<IEnumerable<AllQuestionsViewModel>>(result);

            return ResponseViewModel<IEnumerable<AllQuestionsViewModel>>.Success(vm);
        }

        [HttpGet]
        [Benchmark]
        public async Task<ResponseViewModel<QuestionViewModel>> GetById(int id)
        {
            var cacheKey = $"question_{id}";
            var cached = await _cache.GetStringAsync(cacheKey);

            QuestionsDto result;

            if (cached != null)
            {
                result = JsonSerializer.Deserialize<QuestionsDto>(cached);
            }
            else
            {
                result = await _service.GetQuestionById(id);

                if (result == null)
                    return ResponseViewModel<QuestionViewModel>
                        .Failure("Question not found.", ErrorCode.QuestionNotFound);

                await _cache.SetStringAsync(
                    cacheKey,
                    JsonSerializer.Serialize(result),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                    }
                );
            }

            var vm = _mapper.Map<QuestionViewModel>(result);

            return ResponseViewModel<QuestionViewModel>.Success(vm);
        }

        [HttpPost]
        public async Task<ResponseViewModel<bool>> Add(AddQuestionViewModel vm)
        {
            var dto = _mapper.Map<AddQuestionsDto>(vm);

            var success = await _service.AddQuestion(dto);

            if (!success)
                return ResponseViewModel<bool>
                    .Failure("Invalid question data.", ErrorCode.ValidationFailed);

            BackgroundJob.Enqueue<QuestionJobsService>(job => job.RefreshQuestionsCache());

            return ResponseViewModel<bool>.Success(true);
        }

        [HttpPost]
        public async Task<ResponseViewModel<bool>> Update(UpdateQuestionViewModel vm)
        {
            var dto = _mapper.Map<UpdateQuestionsDto>(vm);

            var success = await _service.UpdateQuestion(dto);

            if (!success)
                return ResponseViewModel<bool>
                    .Failure("Update failed.", ErrorCode.QuestionNotFound);

            // Refresh questions cache
            BackgroundJob.Enqueue<QuestionJobsService>(job => job.RefreshQuestionsCache());
            
            // Invalidate individual question cache
            BackgroundJob.Enqueue<QuestionJobsService>(job => job.InvalidateQuestionCache(dto.ID));
            
            // Invalidate exam questions cache that contain this question
            BackgroundJob.Enqueue<ExamQuestionJobsService>(job => job.InvalidateCacheForQuestion(dto.ID));

            return ResponseViewModel<bool>.Success(true);
        }

        [HttpDelete]
        public async Task<ResponseViewModel<bool>> Delete(int id)
        {
            var success = await _service.DeleteQuestion(id);

            if (!success)
                return ResponseViewModel<bool>
                    .Failure("Question not found.", ErrorCode.QuestionNotFound);

            // Refresh questions cache
            BackgroundJob.Enqueue<QuestionJobsService>(job => job.RefreshQuestionsCache());
            
            // Invalidate individual question cache
            BackgroundJob.Enqueue<QuestionJobsService>(job => job.InvalidateQuestionCache(id));
            
            // Invalidate exam questions cache that contain this question
            BackgroundJob.Enqueue<ExamQuestionJobsService>(job => job.InvalidateCacheForQuestion(id));

            return ResponseViewModel<bool>.Success(true);
        }
    }
}
