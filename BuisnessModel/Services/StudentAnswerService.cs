using AutoMapper;
using BuisnessModel.DTOs.StudentAnswer;
using BuisnessModel.Interfaces;
using DataAccess.Models;

namespace BuisnessModel.Services
{
    public class StudentAnswerService
    {
        private readonly IStudentAnswerRepository _answerRepository;
        private readonly IStudentExamRepository _studentExamRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IChoiceRepository _choiceRepository;
        private readonly IMapper _mapper;

        public StudentAnswerService(
            IStudentAnswerRepository answerRepository,
            IStudentExamRepository studentExamRepository,
            IQuestionRepository questionRepository,
            IChoiceRepository choiceRepository,
            IMapper mapper)
        {
            _answerRepository = answerRepository;
            _studentExamRepository = studentExamRepository;
            _questionRepository = questionRepository;
            _choiceRepository = choiceRepository;
            _mapper = mapper;
        }

        public async Task<(bool IsSuccess, string ErrorMessage)> SubmitAnswerAsync(StudentAnswerAddDto dto)
        {
            var studentExam = await _studentExamRepository.GetByID(dto.StudentExamId);
            if (studentExam == null)
                return (false, "Student exam not found");

            var question = await _questionRepository.GetByID(dto.QuestionId);
            if (question == null)
                return (false, "Question not found");

            if (dto.ChoiceId.HasValue)
            {
                var choice = await _choiceRepository.GetByID(dto.ChoiceId.Value);
                if (choice == null || choice.QuestionId != dto.QuestionId)
                    return (false, "Invalid choice");
            }

            var answer = _mapper.Map<StudentAnswer>(dto);

            if (dto.ChoiceId.HasValue)
            {
                var correctChoice = await _choiceRepository.GetCorrectChoiceForQuestionAsync(dto.QuestionId);
                answer.IsCorrect = correctChoice?.ID == dto.ChoiceId.Value;
            }
            else
            {
                answer.IsCorrect = false; 
            }

            var saved = await _answerRepository.SaveStudentAnswerAsync(answer);
            if (!saved)
                return (false, "Failed to save answer");

            return (true, string.Empty);
        }
    }
}
