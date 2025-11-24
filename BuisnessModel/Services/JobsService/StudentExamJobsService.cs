using BuisnessModel.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BuisnessModel.Services.JobsService
{
    public class StudentExamJobsService
    {
        private readonly IStudentExamRepository _studentExamRepo;
        private readonly IStudentAnswerRepository _studentAnswerRepo;

        public StudentExamJobsService(
            IStudentExamRepository studentExamRepo,
            IStudentAnswerRepository studentAnswerRepo)
        {
            _studentExamRepo = studentExamRepo;
            _studentAnswerRepo = studentAnswerRepo;
        }

        public async Task CalculateExamScore(int studentExamId)
        {
            var studentExam = await _studentExamRepo.GetByID(studentExamId);
            if (studentExam == null || studentExam.IsCompleted)
                return;

            var answers = await _studentAnswerRepo.GetByStudentExamIdAsync(studentExamId);
            
            var totalQuestions = answers.Count();
            var correctAnswers = answers.Count(a => a.IsCorrect);

            studentExam.Score = totalQuestions > 0 
                ? (double)correctAnswers / totalQuestions * 100 
                : 0;
            
            studentExam.IsCompleted = true;
            studentExam.AttemptedAt = DateTime.UtcNow;

            await _studentExamRepo.Update(studentExam);
        }

        public async Task AutoCompleteExpiredExams()
        {
            // Get all exams where EndTime has passed but IsCompleted = false
            var expiredExams = _studentExamRepo.GetAll()
                .Where(se => !se.IsCompleted 
                    && se.Exam.EndTime.HasValue 
                    && se.Exam.EndTime < DateTime.UtcNow)
                .ToList();

            foreach (var exam in expiredExams)
            {
                await CalculateExamScore(exam.ID);
            }
        }
    }
}