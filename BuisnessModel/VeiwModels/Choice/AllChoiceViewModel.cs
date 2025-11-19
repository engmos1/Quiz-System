using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessModel.VeiwModels.Choice
{
    public class AllChoiceViewModel
    {
        public int QuestionId { get; set; }

        public string Text { get; set; }

        public bool IsCorrect { get; set; }
    }
}
