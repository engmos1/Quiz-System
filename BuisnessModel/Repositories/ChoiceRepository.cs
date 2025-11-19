using BuisnessModel.Interfaces;
using DataAccess.Context;
using DataAccess.Models;
using ExaminationSystem.Repositories;
using Microsoft.EntityFrameworkCore;


namespace BuisnessModel.Repositories
{
    public class ChoiceRepository : GeneralRepository<Choice> , IChoiceRepository
    {
        QuizSystemContext _context;
        public ChoiceRepository(QuizSystemContext context) : base(context)
        {
            _context = context;
        }
        public IQueryable<Choice> GetAllByQuestionId(int questionId)
        {
            var res = _context.Choices.Where(x => !x.IsDeleted && x.QuestionId == questionId);
            return res;
        }
    }
}
