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
            CreateMap<Exam, ExamsDTO>().ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.Course.ID))
                .ForMember(dest => dest.InctractorId , opt => opt.MapFrom(src=>src.CreatedByInstructorId))
                .ReverseMap();
            CreateMap<Exam, AddExamDto>()
                .ForMember(dest=> dest.InctractorId,opt=> opt.MapFrom(src=>src.CreatedByInstructorId)).ReverseMap();
            CreateMap<Exam, UpdateExamsDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.InctractorId, opt => opt.MapFrom(src => src.CreatedByInstructorId)).ReverseMap();





            // For Veiw Models
            CreateMap< AllExamsDTO , AllExamsVeiwModels>().ReverseMap();
            CreateMap<ExamsDTO , ExamsVeiwModels>().ReverseMap();
            CreateMap<AddExamDto, AddExamsVeiwModels>().ReverseMap();
            CreateMap<UpdateExamsDto, UpdateExamsVeiwModels>().ReverseMap();
        }
    }
}
