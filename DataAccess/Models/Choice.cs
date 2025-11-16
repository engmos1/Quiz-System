using ExaminationSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class Choice: BaseModel
    {
        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
