using AutoMapper;
using BuisnessModel.DTOs.Exam;
using BuisnessModel.VeiwModels.Exam;
using DataAccess.Models.Enums;
using ExaminationSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

[Route("[controller]/[action]")]
[ApiController]
public class ExamController : ControllerBase
{
    private readonly ExamService _service;
    private readonly IMapper _mapper;

    public ExamController(ExamService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ResponseViewModel<IEnumerable<AllExamsVeiwModels>>> GetAllExams()
    {
        var result = await _service.GetAll();

        if (result.Count == 0)
            return ResponseViewModel<IEnumerable<AllExamsVeiwModels>>
                .Failure("No exams found", ErrorCode.ExamNotFound);

        var vm = _mapper.Map<IEnumerable<AllExamsVeiwModels>>(result);
        return ResponseViewModel<IEnumerable<AllExamsVeiwModels>>.Success(vm);
    }

    [HttpGet]
    public async Task<ResponseViewModel<ExamsVeiwModels>> GetExamById(int id)
    {
        var result = await _service.GetById(id);

        if (result == null)
            return ResponseViewModel<ExamsVeiwModels>
                .Failure("Exam not found", ErrorCode.ExamNotFound);

        var vm = _mapper.Map<ExamsVeiwModels>(result);
        return ResponseViewModel<ExamsVeiwModels>.Success(vm);
    }

    [HttpPost]
    public async Task<ResponseViewModel<bool>> AddExam(AddExamsVeiwModels vm)
    {
        var dto = _mapper.Map<ExamsDTO>(vm);

        var success = await _service.AddExam(dto);

        if (!success)
            return ResponseViewModel<bool>
                .Failure("Validation failed", ErrorCode.ValidationFailed);

        return ResponseViewModel<bool>.Success(true);
    }

    [HttpPost]
    public async Task<ResponseViewModel<bool>> UpdateExam(AddExamsVeiwModels vm)
    {
        var dto = _mapper.Map<ExamsDTO>(vm);

        var success = await _service.UpdateExam(dto);

        if (!success)
            return ResponseViewModel<bool>
                .Failure("Exam not found or invalid data", ErrorCode.ExamNotFound);

        return ResponseViewModel<bool>.Success(true);
    }

    [HttpDelete]
    public async Task<ResponseViewModel<bool>> DeleteExam(int id)
    {
        var success = await _service.DeleteExam(id);

        if (!success)
            return ResponseViewModel<bool>
                .Failure("Exam not found", ErrorCode.ExamNotFound);

        return ResponseViewModel<bool>.Success(true);
    }
}
