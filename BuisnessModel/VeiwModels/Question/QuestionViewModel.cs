using BuisnessModel.DTOs.Choice;
using DataAccess.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessModel.VeiwModels.Question
{
    public class QuestionViewModel
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public QuestionLevel Level { get; set; }
        public bool IsReusable { get; set; }

        public string CreatedByInstructorId { get; set; }

        public List<ChoiceDto> Choices { get; set; }
    }
}
