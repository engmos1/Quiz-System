using AutoMapper;
using BuisnessModel.Services;
using DataAccess.Models.Enums;
using ExaminationSystem.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Quiz_System.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CourseAssignmentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly CourseAssignmentService _courseAssignmentService;
        public CourseAssignmentController(IMapper mapper, CourseAssignmentService courseAssignmentService)
        {
            _mapper = mapper;
            _courseAssignmentService = courseAssignmentService;

        }

        [HttpPost]
        public async Task<ResponseViewModel<bool>> AssignCourseToStudent(int courseId, string studentId)
        {
            var result = await _courseAssignmentService.AssignCourseToStudent(courseId, studentId);
            if (!result.IsSuccess && result.ErrorMessage == "Course not found.")
            {
                return  ResponseViewModel<bool>.Failure(result.ErrorMessage, ErrorCode.ChoiceNotFound);
            }
            if (!result.IsSuccess && result.ErrorMessage == "Student not found.")
            {
                return ResponseViewModel<bool>.Failure(result.ErrorMessage, ErrorCode.StudentNotFound);
            }
            if (!result.IsSuccess)
            {
                return ResponseViewModel<bool>.Failure(result.ErrorMessage, ErrorCode.UnexpectedError);
            }
            return ResponseViewModel<bool>.Success(true);
        }
    }
}
