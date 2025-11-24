using DataAccess.Models;
using System.Linq.Expressions;


namespace BuisnessModel.Interfaces
{
    public interface IStudentExamRepository : IGeneralRepository<StudentExam>
    {
        Task<bool> IsExist(Expression<Func<StudentExam, bool>> predicate);
        Task<StudentExam> GetWithDetailsAsync(int id);
        Task<IEnumerable<StudentExam>> GetByStudentIdAsync(string studentId);
        Task<IEnumerable<StudentExam>> GetByExamIdAsync(int examId);
        Task<bool> AssignStudentToExamAsync(string studentId, int examId);

    }
}
