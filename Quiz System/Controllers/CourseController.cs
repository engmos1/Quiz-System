using AutoMapper;
using BuisnessModel.DTOs.Course;
using BuisnessModel.Services;
using BuisnessModel.VeiwModels.Course;
using DataAccess.Models.Enums;
using ExaminationSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly CourseService _service;
        private readonly IMapper _mapper;

        public CourseController(CourseService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public ResponseViewModel<IEnumerable<AllCoursesViewModel>> GetAll()
        {
            var result = _service.GetAll();

            if (result.Count == 0)
                return ResponseViewModel<IEnumerable<AllCoursesViewModel>>
                       .Failure("No courses found.", ErrorCode.CourseNotFound);

            var vm = _mapper.Map<IEnumerable<AllCoursesViewModel>>(result);
            return ResponseViewModel<IEnumerable<AllCoursesViewModel>>.Success(vm);
        }


        [HttpGet]
        public async Task<ResponseViewModel<CourseViewModel>> GetById(int id)
        {
            var result = await _service.GetById(id);

            if (result == null)
                return ResponseViewModel<CourseViewModel>
                        .Failure("Course not found.", ErrorCode.CourseNotFound);

            var vm = _mapper.Map<CourseViewModel>(result);
            return ResponseViewModel<CourseViewModel>.Success(vm);
        }

        [HttpPost]
        public async Task<ResponseViewModel<bool>> Add(AddCourseViewModel vm)
        {
            var dto = _mapper.Map<AddCourseDTO>(vm);

            var success = await _service.Add(dto);

            if (!success)
                return ResponseViewModel<bool>
                    .Failure("Invalid course data.", ErrorCode.ValidationFailed);

            return ResponseViewModel<bool>.Success(true);
        }


        [HttpPost]
        public async Task<ResponseViewModel<bool>> Update(CourseViewModel vm)
        {
            var dto = _mapper.Map<CourseDTO>(vm);

            var success = await _service.Update(dto);

            if (!success)
                return ResponseViewModel<bool>
                    .Failure("Update failed.", ErrorCode.CourseNotFound);

            return ResponseViewModel<bool>.Success(true);
        }

        [HttpDelete]
        public async Task<ResponseViewModel<bool>> Delete(int id)
        {
            var success = await _service.Delete(id);

            if (!success)
                return ResponseViewModel<bool>
                    .Failure("Course not found.", ErrorCode.CourseNotFound);

            return ResponseViewModel<bool>.Success(true);
        }
    }
}
