using AutoMapper;
using BuisnessModel.DTOs.ExamQuestion;
using BuisnessModel.Interfaces;
using BuisnessModel.Services;
using BuisnessModel.Services.JobsService;
using BuisnessModel.VeiwModels.ExamQuestion;
using DataAccess.Models.Enums;
using ExaminationSystem.Attributes;
using ExaminationSystem.ViewModels;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Quiz_System.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ExamQuestionController : ControllerBase
    {
        private readonly ExamQuestionService _examQuestionService;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public ExamQuestionController(ExamQuestionService examQuestionService, IMapper mapper, IDistributedCache cache)
        {
            _examQuestionService = examQuestionService;
            _mapper = mapper;
            _cache = cache;
        }

        [HttpGet]
        [Benchmark]
        public async Task<ResponseViewModel<IEnumerable<AllExamQuestionVeiwModel>>> GetQuestionsByExamId(int examId)
        {
            var cacheKey = $"exam_{examId}_questions";
            var cached = await _cache.GetStringAsync(cacheKey);

            IEnumerable<AllExamQuestionDto> questions;

            if (cached != null)
            {
                questions = JsonSerializer.Deserialize<IEnumerable<AllExamQuestionDto>>(cached);
            }
            else
            {
                questions = await _examQuestionService.GetQuestionsByExamId(examId);

                if (!questions.Any())
                    return ResponseViewModel<IEnumerable<AllExamQuestionVeiwModel>>
                        .Failure("No questions found for this exam.", ErrorCode.QuestionNotFound);

                await _cache.SetStringAsync(
                    cacheKey,
                    JsonSerializer.Serialize(questions),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2)
                    }
                );
            }

            var questionsDto = _mapper.Map<IEnumerable<AllExamQuestionVeiwModel>>(questions);
            return ResponseViewModel<IEnumerable<AllExamQuestionVeiwModel>>.Success(questionsDto);
        }

        [HttpPost]
        public async Task<ResponseViewModel<bool>> AddExamQuestion(int examId, int questionId)
        {
            var (isSuccess, errorMessage) = await _examQuestionService.AddExamQuestion(examId, questionId);

            if (!isSuccess)
            {
                return ResponseViewModel<bool>.Failure(errorMessage, ErrorCode.ValidationFailed);
            }

            // Invalidate cache when adding new question to exam
            BackgroundJob.Enqueue<ExamQuestionJobsService>(job => job.InvalidateExamQuestionsCache(examId));

            return ResponseViewModel<bool>.Success(true);
        }
    }
}
