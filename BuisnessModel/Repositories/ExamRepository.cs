using ExaminationSystem.Repositories;
using DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Models;
using BuisnessModel.Interfaces;

namespace BuisnessModel.Repositories
{
    public class ExamRepository : GeneralRepository<Exam>, IExamRepository
    {
        public ExamRepository(QuizSystemContext context) : base(context)
        {
        }
    }
}
