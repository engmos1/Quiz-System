using DataAccess.Identity;
using ExaminationSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class StudentExam : BaseModel
    {
        public int ExamId { get; set; }
        public Exam Exam { get; set; }

        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }

        public DateTime AssignedAt { get; set; }
        public bool IsCompleted { get; set; }
        public double? Score { get; set; }
        public DateTime? AttemptedAt { get; set; }

        public ICollection<StudentAnswer> StudentAnswers { get; set; }
    }
}
