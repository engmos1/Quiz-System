using AutoMapper;
using BuisnessModel.Interfaces;
using BuisnessModel.Services;
using BuisnessModel.VeiwModels.ExamQuestion;
using DataAccess.Models.Enums;
using ExaminationSystem.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Quiz_System.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ExamQuestionController : ControllerBase
    {
        private readonly ExamQuestionService _examQuestionService;
        private readonly IMapper _mapper;
        public ExamQuestionController(ExamQuestionService examQuestionService, IMapper mapper)
        {
            _examQuestionService = examQuestionService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ResponseViewModel<IEnumerable<AllExamQuestionVeiwModel>>> GetQuestionsByExamId(int examId)
        {
            var questions = await _examQuestionService.GetQuestionsByExamId(examId);
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
            return ResponseViewModel<bool>.Success(true);
        }
    }
}
