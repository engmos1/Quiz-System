using AutoMapper;
using BuisnessModel.DTOs.Exam;
using BuisnessModel.Services.JobsService;
using BuisnessModel.VeiwModels.Exam;
using DataAccess.Models.Enums;
using ExaminationSystem.Attributes;
using ExaminationSystem.ViewModels;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

[Route("[controller]/[action]")]
[ApiController]
public class ExamController : ControllerBase
{
    private readonly ExamService _service;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cache;

    public ExamController(ExamService service, IMapper mapper, IDistributedCache cache)
    {
        _service = service;
        _mapper = mapper;
        _cache = cache;
    }

    [HttpGet]
    [Benchmark]
    public async Task<ResponseViewModel<IEnumerable<AllExamsVeiwModels>>> GetAllExams()
    {
        var cacheKey = "exams_all";
        var cached = await _cache.GetStringAsync(cacheKey);

        List<AllExamsDTO> result;

        if (cached != null)
        {
            result = JsonSerializer.Deserialize<List<AllExamsDTO>>(cached);
        }
        else
        {
            result = await _service.GetAll();

            if (result.Count == 0)
                return ResponseViewModel<IEnumerable<AllExamsVeiwModels>>
                    .Failure("No exams found", ErrorCode.ExamNotFound);

            await _cache.SetStringAsync(
                cacheKey,
                JsonSerializer.Serialize(result),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                }
            );
        }

        var vm = _mapper.Map<IEnumerable<AllExamsVeiwModels>>(result);
        return ResponseViewModel<IEnumerable<AllExamsVeiwModels>>.Success(vm);
    }

    [HttpGet]
    [Benchmark]
    public async Task<ResponseViewModel<ExamsVeiwModels>> GetExamById(int id)
    {
        var cacheKey = $"exam_{id}";
        var cached = await _cache.GetStringAsync(cacheKey);

        ExamsDTO result;

        if (cached != null)
        {
            result = JsonSerializer.Deserialize<ExamsDTO>(cached);
        }
        else
        {
            result = await _service.GetById(id);

            if (result == null)
                return ResponseViewModel<ExamsVeiwModels>
                    .Failure("Exam not found", ErrorCode.ExamNotFound);

            await _cache.SetStringAsync(
                cacheKey,
                JsonSerializer.Serialize(result),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                }
            );
        }

        var vm = _mapper.Map<ExamsVeiwModels>(result);
        return ResponseViewModel<ExamsVeiwModels>.Success(vm);
    }

    [HttpPost]
    public async Task<ResponseViewModel<bool>> AddExam(AddExamsVeiwModels vm)
    {
        var dto = _mapper.Map<AddExamDto>(vm);

        var success = await _service.AddExam(dto);

        if (!success)
            return ResponseViewModel<bool>
                .Failure("Validation failed", ErrorCode.ValidationFailed);

        BackgroundJob.Enqueue<ExamJobsService>(job => job.RefreshExamsCache());

        return ResponseViewModel<bool>.Success(true);
    }

    [HttpPost]
    public async Task<ResponseViewModel<bool>> UpdateExam(UpdateExamsVeiwModels vm)
    {
        var dto = _mapper.Map<UpdateExamsDto>(vm);

        var success = await _service.UpdateExam(dto);

        if (!success)
            return ResponseViewModel<bool>
                .Failure("Exam not found or invalid data", ErrorCode.ExamNotFound);

        BackgroundJob.Enqueue<ExamJobsService>(job => job.RefreshExamsCache());
        BackgroundJob.Enqueue<ExamJobsService>(job => job.InvalidateExamCache(dto.Id));

        return ResponseViewModel<bool>.Success(true);
    }

    [HttpDelete]
    public async Task<ResponseViewModel<bool>> DeleteExam(int id)
    {
        var success = await _service.DeleteExam(id);

        if (!success)
            return ResponseViewModel<bool>
                .Failure("Exam not found", ErrorCode.ExamNotFound);

        BackgroundJob.Enqueue<ExamJobsService>(job => job.RefreshExamsCache());
        BackgroundJob.Enqueue<ExamJobsService>(job => job.InvalidateExamCache(id));

        return ResponseViewModel<bool>.Success(true);
    }
}
