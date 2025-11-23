using BuisnessModel.DTOs.Question;
using BuisnessModel.VeiwModels.Question;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessModel.VeiwModels.ExamQuestion
{
    public class AllExamQuestionVeiwModel
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }

        public QuestionViewModel Question { get; set; }
    }
}
