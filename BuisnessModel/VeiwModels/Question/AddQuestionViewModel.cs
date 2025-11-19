using BuisnessModel.VeiwModels.Choice;
using DataAccess.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessModel.VeiwModels.Question
{
    public class AddQuestionViewModel
    {
        public string Text { get; set; }
        public QuestionLevel Level { get; set; }
        public bool IsReusable { get; set; }

        public string CreatedByInstructorId { get; set; }

        public List<AddChoiceViewModel> Choices { get; set; }
    }
}
