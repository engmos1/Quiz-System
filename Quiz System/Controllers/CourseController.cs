using AutoMapper;
using BuisnessModel.DTOs.Course;
using BuisnessModel.Services;
using BuisnessModel.Services.JobsService;
using BuisnessModel.VeiwModels.Course;
using DataAccess.Models.Enums;
using ExaminationSystem.Attributes;
using ExaminationSystem.ViewModels;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace ExaminationSystem.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly CourseService _service;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;


        public CourseController(CourseService service, IMapper mapper, IDistributedCache cache)
        {
            _service = service;
            _mapper = mapper;
            _cache = cache;
        }


        [HttpGet]
        [Benchmark]
        public async Task<ResponseViewModel<IEnumerable<AllCoursesViewModel>>> GetAllRedis()
        {
            var cacheKey = "courses_all";
            var cached = await _cache.GetStringAsync(cacheKey);

            IEnumerable<AllCoursesDTO> result;

            if (cached != null)
            {
                result = JsonSerializer.Deserialize<IEnumerable<AllCoursesDTO>>(cached);
            }
            else
            {
                result = _service.GetAll();

                if (!result.Any())
                    return ResponseViewModel<IEnumerable<AllCoursesViewModel>>
                           .Failure("No courses found.", ErrorCode.CourseNotFound);

                await _cache.SetStringAsync(
                    cacheKey,
                    JsonSerializer.Serialize(result),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    }
                );
            }

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


            BackgroundJob.Enqueue<CourseJobsService>(job => job.RefreshCoursesCache());

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

            // Trigger background refresh
            BackgroundJob.Enqueue<CourseJobsService>(job => job.RefreshCoursesCache());
            return ResponseViewModel<bool>.Success(true);
        }

        [HttpDelete]
        public async Task<ResponseViewModel<bool>> Delete(int id)
        {
            var success = await _service.Delete(id);

            if (!success)
                return ResponseViewModel<bool>
                    .Failure("Course not found.", ErrorCode.CourseNotFound);

            BackgroundJob.Enqueue<CourseJobsService>(job => job.RefreshCoursesCache());

            return ResponseViewModel<bool>.Success(true);
        }
    }
}
