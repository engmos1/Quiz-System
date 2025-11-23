using AutoMapper;
using BuisnessModel.DTOs.ExamQuestion;
using BuisnessModel.VeiwModels.ExamQuestion;
using DataAccess.Models;


namespace BuisnessModel.Mapping
{
    public class ExamQuestionProfile : Profile
    {
        public ExamQuestionProfile()
        {
            // CreateMap<Source, Destination>();
            CreateMap<ExamQuestion, AllExamQuestionDto>();
                //.ForMember(dest => dest.QuestionsDto, opt => opt.MapFrom(src => src.Question));

            CreateMap<AllExamQuestionDto, AllExamQuestionVeiwModel>();
        }
    }
}
