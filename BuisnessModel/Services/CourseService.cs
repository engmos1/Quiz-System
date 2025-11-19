using AutoMapper;
using BuisnessModel.DTOs.Course;
using BuisnessModel.Interfaces;
using DataAccess.Identity;
using DataAccess.Models;
using ExaminationSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace BuisnessModel.Services
{
    public class CourseService
    {
        private readonly IMapper _mapper;
        private readonly ICourseRepository _courseRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public CourseService(
            IMapper mapper,
            ICourseRepository courseRepo,
            UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _courseRepo = courseRepo;
            _userManager = userManager;
        }


        public List<AllCoursesDTO> GetAll()
        {
            var courses = _courseRepo.GetAll();

            var result = (from c in courses
                          select new AllCoursesDTO
                          {
                              Id = c.ID,
                              Name = c.Name,
                              Hours = c.Hours,
                              InstructorName = c.Instructor.FullName
                          }).ToList();

            return result;
        }

        public async Task<CourseDTO?> GetById(int id)
        {
            if (id <= 0)
                return null;

            var course = await _courseRepo.GetByID(id);
            if (course == null)
                return null;

            return _mapper.Map<CourseDTO>(course);
        }


        public async Task<bool> Add(AddCourseDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name) || dto.Hours <= 0)
                return false;

            var instructor = await _userManager.FindByIdAsync(dto.InstructorId);
            if (instructor == null)
                return false;

            var isInstructor = await _userManager.IsInRoleAsync(instructor, "Instructor");
            if (!isInstructor)
                return false;

            var entity = _mapper.Map<Course>(dto);
            _courseRepo.Add(entity);

            return true;
        }


        public async Task<bool> Update(CourseDTO dto)
        {
            if (dto.Id <= 0)
                return false;

            var existing = await _courseRepo.GetByID(dto.Id);
            if (existing == null)
                return false;

            var instructor = await _userManager.FindByIdAsync(dto.InstructorId);
            if (instructor == null)
                return false;

            var isInstructor = await _userManager.IsInRoleAsync(instructor, "Instructor");
            if (!isInstructor)
                return false;

            // عمل entity عادي من الـ DTO
            var entity = _mapper.Map<Course>(dto);

            // تحديد الخصائص اللي اتعدلت
            var modifiedFields = new List<string>();

            if (existing.Name != dto.Name)
                modifiedFields.Add(nameof(Course.Name));

            if (existing.Hours != dto.Hours)
                modifiedFields.Add(nameof(Course.Hours));

            if (existing.InstructorId != dto.InstructorId)
                modifiedFields.Add(nameof(Course.InstructorId));

            // لو مفيش حاجة اتغيرت يبقى خلاص
            if (modifiedFields.Count == 0)
                return true;

            // استدعاء الميثود اللي عندك في الريبو
            _courseRepo.UpdateInclude(entity, modifiedFields.ToArray());

            return true;
        }


        public async Task<bool> Delete(int id)
        {
            if (id <= 0)
                return false;

            var course = await _courseRepo.GetByID(id);
            if (course == null)
                return false;

            _courseRepo.Delete(id);
            return true;
        }
    }
}
