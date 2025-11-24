using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BuisnessModel.VeiwModels.StudentAnswer
{
    public class StudentAnswerAddViewModel
    {
        [Required]
        public int StudentExamId { get; set; }

        [Required]
        public int QuestionId { get; set; }

        public int? ChoiceId { get; set; }
    }
}
