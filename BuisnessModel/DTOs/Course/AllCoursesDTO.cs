using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessModel.DTOs.Course
{
    public class AllCoursesDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Hours { get; set; }

        public string InstructorName { get; set; }
    }
}
