using BuisnessModel.DTOs.Choice;
using DataAccess.Identity;
using DataAccess.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessModel.DTOs.Question
{
    public class AllQuestionsDto
    {
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public QuestionLevel Level { get; set; }
        public bool IsReusable { get; set; }

        public string CreatedByInstructorId { get; set; }

        public List<AllChoiceDto> Choices { get; set; }
    }
}
