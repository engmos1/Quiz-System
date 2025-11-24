using DataAccess.Models;


namespace BuisnessModel.Interfaces
{
    public interface IStudentAnswerRepository : IGeneralRepository<StudentAnswer>
    {
        Task<IEnumerable<StudentAnswer>> GetByStudentExamIdAsync(int studentExamId);
        Task<StudentAnswer> GetWithDetailsAsync(int id);
        Task<bool> SaveStudentAnswerAsync(StudentAnswer answer);
    }
}
