using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace BuisnessModel.Services.JobsService
{
    public class QuestionJobsService
    {
        private readonly QuestionService _service;
        private readonly IDistributedCache _cache;

        public QuestionJobsService(QuestionService service, IDistributedCache cache)
        {
            _service = service;
            _cache = cache;
        }

        public async Task RefreshQuestionsCache()
        {
            var result = await _service.GetAllQuestions();
            if (!result.Any()) return;

            var json = JsonSerializer.Serialize(result);

            await _cache.SetStringAsync(
                "questions_all",
                json,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                }
            );
        }

        public async Task InvalidateQuestionCache(int questionId)
        {
            var cacheKey = $"question_{questionId}";
            await _cache.RemoveAsync(cacheKey);
        }

        public async Task RefreshQuestionCache(int questionId)
        {
            var result = await _service.GetQuestionById(questionId);
            if (result == null) return;

            var cacheKey = $"question_{questionId}";
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