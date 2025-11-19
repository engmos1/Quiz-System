using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessModel.VeiwModels.Choice
{
    public class UpdateChoiceViewModel
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }

        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
