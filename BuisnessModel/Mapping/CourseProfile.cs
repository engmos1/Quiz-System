using AutoMapper;
using BuisnessModel.DTOs.Course;
using BuisnessModel.VeiwModels.Course;
using ExaminationSystem.Models;


namespace BuisnessModel.Mapping
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseDTO>();
            CreateMap<Course, AllCoursesDTO>()
                .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.Instructor.FullName));

            // DTO → Entity
            CreateMap<CourseDTO, Course>();
            CreateMap<AddCourseDTO, Course>();

            // DTO ↔ ViewModel
            CreateMap<CourseDTO, CourseViewModel>().ReverseMap();
            CreateMap<AddCourseDTO, AddCourseViewModel>().ReverseMap();
            CreateMap<AllCoursesDTO, AllCoursesViewModel>().ReverseMap();

        }
    }
}
