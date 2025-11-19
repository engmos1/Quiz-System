using AutoMapper;
using BuisnessModel.DTOs.Choice;
using BuisnessModel.Services;
using BuisnessModel.VeiwModels.Choice;
using DataAccess.Models.Enums;
using ExaminationSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ChoiceController : ControllerBase
    {
        private readonly ChoiceService _service;
        private readonly IMapper _mapper;

        public ChoiceController(ChoiceService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public ResponseViewModel<IEnumerable<AllChoiceViewModel>> GetAllByQuestionId(int questionId)
        {
            var result = _service.GetAllByQuestionId(questionId);

            if (!result.Any())
                return ResponseViewModel<IEnumerable<AllChoiceViewModel>>
                    .Failure("No choices found.", ErrorCode.ChoiceNotFound);

            var vm = _mapper.Map<IEnumerable<AllChoiceViewModel>>(result);

            return ResponseViewModel<IEnumerable<AllChoiceViewModel>>.Success(vm);
        }

        [HttpGet]
        public async Task<ResponseViewModel<ChoiceViewModel>> GetById(int id)
        {
            var result = await _service.GetById(id);

            if (result == null)
                return ResponseViewModel<ChoiceViewModel>
                    .Failure("Choice not found.", ErrorCode.ChoiceNotFound);

            var vm = _mapper.Map<ChoiceViewModel>(result);

            return ResponseViewModel<ChoiceViewModel>.Success(vm);
        }


        //[HttpPost]
        //public ResponseViewModel<bool> AddChoices(IEnumerable<AddChoiceViewModel> vms)
        //{
        //    var dto = _mapper.Map<IEnumerable<AddChoiceDto>>(vms);

        //    var success = _service.AddChoices(dto);

        //    if (!success)
        //        return ResponseViewModel<bool>
        //            .Failure("Invalid choice data.", ErrorCode.ValidationFailed);

        //    return ResponseViewModel<bool>.Success(true);
        //}


        [HttpPost]
        public async Task<ResponseViewModel<bool>> Update(UpdateChoiceViewModel vm)
        {
            var dto = _mapper.Map<UpdateChoiceDto>(vm);

            var success = await Task.Run(() => _service.UpdateChoice(dto));

            if (!success)
                return ResponseViewModel<bool>
                    .Failure("Update failed.", ErrorCode.ChoiceNotFound);

            return ResponseViewModel<bool>.Success(true);
        }


        //[HttpDelete]
        //public ResponseViewModel<bool> Delete(int id)
        //{
        //    var success = _service.DeleteChoice(id);

        //    if (!success)
        //        return ResponseViewModel<bool>
        //            .Failure("Choice not found.", ErrorCode.ChoiceNotFound);

        //    return ResponseViewModel<bool>.Success(true);
        //}
    }
}
