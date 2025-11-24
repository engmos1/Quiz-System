using DataAccess.Models;


namespace BuisnessModel.Interfaces
{
    public interface IChoiceRepository : IGeneralRepository<Choice>
    {
        IQueryable<Choice> GetAllByQuestionId(int questionId);
        Task<Choice?> GetCorrectChoiceForQuestionAsync(int questionId);

    }
}
