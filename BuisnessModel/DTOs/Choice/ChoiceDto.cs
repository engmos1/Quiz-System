using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessModel.DTOs.Choice
{
    public class ChoiceDto
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }

        public string Text { get; set; }
        public bool IsCorrect { get; set; }

    }
}
