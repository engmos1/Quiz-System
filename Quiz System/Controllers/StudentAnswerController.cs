using AutoMapper;
using BuisnessModel.DTOs.StudentAnswer;
using BuisnessModel.Services;
using BuisnessModel.Services.JobsService;
using BuisnessModel.VeiwModels.StudentAnswer;
using DataAccess.Models.Enums;
using ExaminationSystem.ViewModels;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Quiz_System.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class StudentAnswerController : ControllerBase
    {
        private readonly StudentAnswerService _studentAnswerService;
        private readonly IMapper _mapper;

        public StudentAnswerController(StudentAnswerService studentAnswerService, IMapper mapper)
        {
            _studentAnswerService = studentAnswerService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ResponseViewModel<bool>> Submit([FromBody] StudentAnswerAddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return ResponseViewModel<bool>.Failure("Invalid model state", ErrorCode.StudentAnswerInvalid);
            }

            var dto = _mapper.Map<StudentAnswerAddDto>(model);

            var result = await _studentAnswerService.SubmitAnswerAsync(dto);

            if (!result.IsSuccess)
                return ResponseViewModel<bool>.Failure(result.ErrorMessage, ErrorCode.StudentAnswerNotFound);

            return ResponseViewModel<bool>.Success(true);
        }

        [HttpPost]
        public async Task<ResponseViewModel<bool>> CompleteExam(int studentExamId)
        {
            // Trigger score calculation in background
            BackgroundJob.Enqueue<StudentExamJobsService>(
                job => job.CalculateExamScore(studentExamId));

            return ResponseViewModel<bool>.Success(true);
        }
    }
}
