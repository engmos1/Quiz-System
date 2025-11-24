using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace BuisnessModel.Services.JobsService
{
    public class ExamJobsService
    {
        private readonly ExamService _service;
        private readonly IDistributedCache _cache;

        public ExamJobsService(ExamService service, IDistributedCache cache)
        {
            _service = service;
            _cache = cache;
        }

        public async Task RefreshExamsCache()
        {
            var result = await _service.GetAll();
            if (result.Count == 0) return;

            var json = JsonSerializer.Serialize(result);

            await _cache.SetStringAsync(
                "exams_all",
                json,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                }
            );
        }

        public async Task InvalidateExamCache(int examId)
        {
            var cacheKey = $"exam_{examId}";
            await _cache.RemoveAsync(cacheKey);
        }

        public async Task RefreshExamCache(int examId)
        {
            var result = await _service.GetById(examId);
            if (result == null) return;

            var cacheKey = $"exam_{examId}";
            var json = JsonSerializer.Serialize(result);

            await _cache.SetStringAsync(
                cacheKey,
                json,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                }
            );
        }
    }
}