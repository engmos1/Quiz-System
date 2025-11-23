using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace BuisnessModel.Services.JobsService
{
    public class CourseJobsService
    {
        private readonly CourseService _service;
        private readonly IDistributedCache _cache;

        public CourseJobsService(CourseService service, IDistributedCache cache)
        {
            _service = service;
            _cache = cache;
        }

        public async Task RefreshCoursesCache()
        {
            var result = _service.GetAll();
            if (!result.Any()) return;

            var json = JsonSerializer.Serialize(result);

            await _cache.SetStringAsync(
                "courses_all",
                json,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                }
            );
        }

    }
}
