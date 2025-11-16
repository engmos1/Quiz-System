using AutoMapper;
using BuisnessModel.DTOs.Exam;
using BuisnessModel.VeiwModels.Exam;
using ExaminationSystem.Models;


namespace BuisnessModel.Mapping
{
    public class ExamProfile : Profile
    {
        public ExamProfile()
        {
            // For Models
            CreateMap<Exam, AllExamsDTO>()
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name))
                .ForMember(dest => dest.CreatedByInstructorName, opt => opt.MapFrom(src => src.CreatedByInstructor.FullName));
            CreateMap<Exam, ExamsDTO>().ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name))
                .ForMember(dest => dest.CreatedByInstructorName, opt => opt.MapFrom(src => src.CreatedByInstructor.FullName)).ReverseMap();




            // For Veiw Models
            CreateMap< AllExamsDTO , AllExamsVeiwModels>().ReverseMap();
            CreateMap<ExamsDTO , ExamsVeiwModels>().ReverseMap();
            CreateMap<ExamsDTO, AddExamsVeiwModels>().ReverseMap();
        }
    }
}
