using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessModel.VeiwModels.Course
{
    public class CourseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Hours { get; set; }

        public string InstructorId { get; set; }
        public string InstructorName { get; set; }
    }
}
