using BuisnessModel.DTOs.ExamQuestion;
using BuisnessModel.DTOs.Question;
using BuisnessModel.Interfaces;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessModel.Services
{
    public class ExamQuestionService
    {
        private readonly AutoMapper.IMapper _mapper;
        private readonly IExamQuestionRepository _examQuestionRepo;
        private readonly QuestionService _questionRepo;

        public ExamQuestionService(AutoMapper.IMapper mapper, IExamQuestionRepository examQuestionRepo, QuestionService questionRepo)
        {
            _mapper = mapper;
            _examQuestionRepo = examQuestionRepo;
            _questionRepo = questionRepo;
        }

        public async Task<IEnumerable<AllExamQuestionDto>> GetQuestionsByExamId(int examId)
        {
            var questions = await _examQuestionRepo.GetQuestionsByExamId(examId);

            var result = _mapper.Map<IEnumerable<AllExamQuestionDto>>(questions);

            foreach (var item in result)
            {
                var q = await _questionRepo.GetQuestionById(item.QuestionId);

                item.Question = _mapper.Map<QuestionsDto>(q);
            }
            return result;
        }

        public async Task<(bool IsSuccess, string ErrorMessage)> AddExamQuestion(int ExamId , int QuestionId)
        {
            var validation = await _examQuestionRepo.ValidateExamQuestion(ExamId, QuestionId);
            if (!validation.IsSuccess)
            {
                return (false, validation.ErrorMessage);
            }
            var examQuestion = new ExamQuestion
            {
                ExamId = ExamId,
                QuestionId = QuestionId
            };
            await _examQuestionRepo.Add(examQuestion);

            return (true, string.Empty);
        }
    }
}
