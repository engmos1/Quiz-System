using AutoMapper;
using BuisnessModel.DTOs.Choice;
using BuisnessModel.DTOs.Question;
using BuisnessModel.Interfaces;
using BuisnessModel.Repositories;
using DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuisnessModel.Services
{
    public class QuestionService
    {
        private readonly IMapper _mapper;
        private readonly IQuestionRepository _questionRepo;
        private readonly IChoiceRepository _choiceRepository;

        public QuestionService(IMapper mapper, IQuestionRepository questionRepo, IChoiceRepository choiceRepository)
        {
            _mapper = mapper;
            _questionRepo = questionRepo;
            _choiceRepository = choiceRepository;
        }

        public async Task<IEnumerable<AllQuestionsDto>> GetAllQuestions()
        {
            var questions = _questionRepo.GetAll();

            var result = _mapper.Map<List<AllQuestionsDto>>(questions);

            foreach (var q in result)
            {
                var choices = _choiceRepository.GetAllByQuestionId(q.QuestionId);
                foreach (var choice in choices)
                {
                    q.Choices.Add(_mapper.Map<AllChoiceDto>(choice));
                }
                
            }

            return result;
        }

        public async Task<QuestionsDto?> GetQuestionById(int id)
        {
            if (id <= 0)
                return null;

            var question = await _questionRepo.GetByID(id);
            if (question == null)
                return null;
            var choices = _choiceRepository.GetAllByQuestionId(id);
            question.Choices = choices.ToList();

            return _mapper.Map<QuestionsDto>(question);
        }


        public async Task<bool> AddQuestion(AddQuestionsDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Text))
                return false;

            var questionEntity = _mapper.Map<Question>(dto);

            questionEntity.Choices = null;

             await _questionRepo.Add(questionEntity);

            if (dto.Choices != null && dto.Choices.Count > 0)
            {
                foreach (var choiceDto in dto.Choices)
                {
                    var choice = _mapper.Map<Choice>(choiceDto);

                    choice.QuestionId = questionEntity.ID;

                   await _choiceRepository.Add(choice);
                }
            }

            return true;
        }


        public async Task<bool> UpdateQuestion(UpdateQuestionsDto dto)
        {
            if (dto.ID <= 0)
                return false;

            var existing = await _questionRepo.GetByID(dto.ID);
            if (existing == null)
                return false;

            var entity = _mapper.Map<Question>(dto);

            var modifiedFields = new List<string>();

            if (existing.Text != dto.Text)
                modifiedFields.Add(nameof(Question.Text));

            if (existing.Level != dto.Level)
                modifiedFields.Add(nameof(Question.Level));

            if (existing.IsReusable != dto.IsReusable)
                modifiedFields.Add(nameof(Question.IsReusable));

            if (existing.CreatedByInstructorId != dto.CreatedByInstructorId)
                modifiedFields.Add(nameof(Question.CreatedByInstructorId));

            if (modifiedFields.Count == 0)
                return true;

            _questionRepo.UpdateInclude(entity, modifiedFields.ToArray());

            return true;
        }


        public async Task<bool> DeleteQuestion(int id)
        {
            if (id <= 0)
                return false;

            var existing = await _questionRepo.GetByID(id);
            if (existing == null)
                return false;

            await _questionRepo.Delete(id);
            return true;
        }
    }
}
