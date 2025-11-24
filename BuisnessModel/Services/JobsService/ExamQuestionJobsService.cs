using BuisnessModel.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Linq;
using System.Threading.Tasks;

namespace BuisnessModel.Services.JobsService
{
    public class ExamQuestionJobsService
    {
        private readonly IDistributedCache _cache;
        private readonly IExamQuestionRepository _examQuestionRepo;

        public ExamQuestionJobsService(IDistributedCache cache, IExamQuestionRepository examQuestionRepo)
        {
            _cache = cache;
            _examQuestionRepo = examQuestionRepo;
        }

        public async Task InvalidateExamQuestionsCache(int examId)
        {
            var cacheKey = $"exam_{examId}_questions";
            await _cache.RemoveAsync(cacheKey);
        }

        public async Task InvalidateCacheForQuestion(int questionId)
        {
            // Get all exams that contain this question
            var examQuestions = await _examQuestionRepo.GetQuestionsByExamId(questionId);
            
            foreach (var eq in examQuestions)
            {
                var cacheKey = $"exam_{eq.ExamId}_questions";
                await _cache.RemoveAsync(cacheKey);
            }
        }
    }
}