using AutoMapper;
using BuisnessModel.DTOs.Choice;
using BuisnessModel.Interfaces;
using DataAccess.Models;
using System.Threading.Tasks;

namespace BuisnessModel.Services
{
    public class ChoiceService
    {
        private readonly IChoiceRepository _choiceRepository;
        private readonly IMapper _mapper;

        public ChoiceService(IChoiceRepository choiceRepository, IMapper mapper)
        {
            _choiceRepository = choiceRepository;
            _mapper = mapper;
        }


        public IEnumerable<AllChoiceDto> GetAllByQuestionId(int questionId)
        {
            if (questionId <= 0)
                return Enumerable.Empty<AllChoiceDto>();

            var all = _choiceRepository.GetAllByQuestionId(questionId);

            return _mapper.Map<IEnumerable<AllChoiceDto>>(all);
        }

        public async Task<ChoiceDto?> GetById(int id)
        {
            if (id <= 0)
                return null;

            var choice = await _choiceRepository.GetByID(id);

            if (choice == null)
                return null;

            return _mapper.Map<ChoiceDto>(choice);
        }
        public bool AddChoices(IEnumerable<AddChoiceDto> dtos)
        {
            if (dtos == null || !dtos.Any())
                return false;

            var entities = _mapper.Map<IEnumerable<Choice>>(dtos);

            foreach (var choice in entities)
                _choiceRepository.Add(choice);

            return true;
        }


        public async Task<bool> UpdateChoice(UpdateChoiceDto dto)
        {
            if (dto.Id <= 0)
                return false;

            var existing = await _choiceRepository.GetByID(dto.Id);
            if (existing == null)
                return false;

            var entity = _mapper.Map<Choice>(dto);

            var modifiedFields = new List<string>();

            if (existing.Text != dto.Text)
                modifiedFields.Add(nameof(Choice.Text));

            if (existing.IsCorrect != dto.IsCorrect)
                modifiedFields.Add(nameof(Choice.IsCorrect));

            if (existing.QuestionId != dto.QuestionId)
                modifiedFields.Add(nameof(Choice.QuestionId));

            if (modifiedFields.Count == 0)
                return true;

            _choiceRepository.UpdateInclude(entity, modifiedFields.ToArray());

            return true;
        }

        public bool DeleteChoice(int id)
        {
            if (id <= 0)
                return false;

            var existing = _choiceRepository.GetByID(id);
            if (existing == null)
                return false;

            _choiceRepository.Delete(id);
            return true;
        }
    }
}
