using DataAccess.Identity;
using ExaminationSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class CourseAssignment : BaseModel
    {
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }

        public DateTime AssignedAt { get; set; }
    }
}
