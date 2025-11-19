using AutoMapper;
using BuisnessModel.DTOs.Question;
using BuisnessModel.Services;
using BuisnessModel.VeiwModels.Question;
using DataAccess.Models.Enums;
using ExaminationSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Quiz_System.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly QuestionService _service;
        private readonly IMapper _mapper;

        public QuestionController(QuestionService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ResponseViewModel<IEnumerable<AllQuestionsViewModel>>> GetAll()
        {
            var result = await _service.GetAllQuestions();

            if (!result.Any())
                return ResponseViewModel<IEnumerable<AllQuestionsViewModel>>
                    .Failure("No questions found.", ErrorCode.QuestionNotFound);

            var vm = _mapper.Map<IEnumerable<AllQuestionsViewModel>>(result);

            return ResponseViewModel<IEnumerable<AllQuestionsViewModel>>.Success(vm);
        }


        [HttpGet]
        public async Task<ResponseViewModel<QuestionViewModel>> GetById(int id)
        {
            var result = await _service.GetQuestionById(id);

            if (result == null)
                return ResponseViewModel<QuestionViewModel>
                    .Failure("Question not found.", ErrorCode.QuestionNotFound);

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

            return ResponseViewModel<bool>.Success(true);
        }


        [HttpDelete]
        public async Task<ResponseViewModel<bool>> Delete(int id)
        {
            var success = await _service.DeleteQuestion(id);

            if (!success)
                return ResponseViewModel<bool>
                    .Failure("Question not found.", ErrorCode.QuestionNotFound);

            return ResponseViewModel<bool>.Success(true);
        }
    }
}
