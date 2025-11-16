using BuisnessModel.Interfaces;
using DataAccess.Context;
using DataAccess.Models;
using ExaminationSystem.Repositories;


namespace BuisnessModel.Repositories
{
    internal class ChoiceRepository : GeneralRepository<Choice> , IChoiceRepository
    {
        public ChoiceRepository(QuizSystemContext context) : base(context)
        {
        }
    }
}
