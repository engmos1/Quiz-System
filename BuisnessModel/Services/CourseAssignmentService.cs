using AutoMapper;
using BuisnessModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessModel.Services
{
    public class CourseAssignmentService
    {
        private readonly IMapper _mapper;
        private readonly ICourseAssignmentRepository _courseAssignmentRepo;

        public CourseAssignmentService(IMapper mapper, ICourseAssignmentRepository courseAssignmentRepo)
        {
            _mapper = mapper;
            _courseAssignmentRepo = courseAssignmentRepo;
        }

        public async Task<(bool IsSuccess, string ErrorMessage)> AssignCourseToStudent(int courseId, string studentId)
        {
            var validationResult = await _courseAssignmentRepo.ValidateCourseAssignment(courseId, studentId);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            var existingAssignment = _courseAssignmentRepo.GetByCourseAndStudent(courseId, studentId);
            if (existingAssignment != null)
            {
                return (false, "Course is already assigned to the student.");
            }

            var courseAssignment = new DataAccess.Models.CourseAssignment
            {
                CourseId = courseId,
                StudentId = studentId,
                AssignedAt = DateTime.UtcNow
            };
            await _courseAssignmentRepo.Add(courseAssignment);
             return (true, string.Empty);
        }
    }
}
