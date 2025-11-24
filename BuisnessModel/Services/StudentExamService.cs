using BuisnessModel.Interfaces;
using BuisnessModel.Repositories;
using DataAccess.Models.Enums;
using ExaminationSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessModel.Services
{
    public class StudentExamService
    {
        private readonly IStudentExamRepository _studentExamRepository;
        private readonly IExamRepository _examRepository;
        private readonly UserRepository _userRepository;
        public StudentExamService(IStudentExamRepository studentExamRepository, IExamRepository examRepository, UserRepository userRepository)
        {
            _studentExamRepository = studentExamRepository;
            _examRepository = examRepository;
            _userRepository = userRepository;
        }
        public async Task<(bool Success, string Message)> AssignStudentToExamAsync(string StudentId, int ExamId)
        {
            // 1) Check exam exists
            var exam = await _examRepository.GetByID(ExamId);
            if (exam == null)
                return (false, "Exam not found");

            // 2) Check student exists
            var student = await _userRepository.GetByIdAsync(StudentId);
            if (student == null)
                return (false, "Student not found");

            // 3) Check not already assigned
            var existed = await _studentExamRepository
                .IsExist(se => se.StudentId == StudentId && se.ExamId == ExamId);

            if (existed)
            {
                return (false, "Student already assigned to exam");
            }

            // 4) Assign student
            var result = await _studentExamRepository.AssignStudentToExamAsync(StudentId, ExamId);

            if (!result)
                return (false, "Failed to assign student");

            return (true, "Student assigned successfully");
        }
    }
}


