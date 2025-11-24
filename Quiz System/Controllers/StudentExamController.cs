using BuisnessModel.Services;
using DataAccess.Models.Enums;
using ExaminationSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Quiz_System.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class StudentExamController : ControllerBase
    {
        private readonly StudentExamService _studentExamService;

        public StudentExamController(StudentExamService studentExamService)
        {
            _studentExamService = studentExamService;
        }

        [HttpPost]
        public async Task<ResponseViewModel<bool>> Assign(string StudentId, int ExamId)
        {
            if (!ModelState.IsValid)
            {
                return (ResponseViewModel<bool>.Failure("Validation failed",
                    ErrorCode.ValidationFailed));
            }


            var result = await _studentExamService.AssignStudentToExamAsync(StudentId, ExamId);

            if (!result.Success)
                return ResponseViewModel<bool>.Failure(result.Message, ErrorCode.StudentExamInvalid);

            return ResponseViewModel<bool>.Success(true);
        }
    }
}
