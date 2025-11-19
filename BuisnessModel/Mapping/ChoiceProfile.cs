using AutoMapper;
using BuisnessModel.DTOs.Choice;
using BuisnessModel.VeiwModels.Choice;
using DataAccess.Models;


namespace BuisnessModel.Mapping
{
    public class ChoiceProfile : Profile
    {
        public ChoiceProfile()
        {
            // DTOS <===> Models
            CreateMap<Choice, AllChoiceDto>()
                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.IsCorrect, opt => opt.MapFrom(src => src.IsCorrect)).ReverseMap();
            CreateMap<AddChoiceDto, Choice>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.IsCorrect, opt => opt.MapFrom(src => src.IsCorrect)).ReverseMap();
            CreateMap<UpdateChoiceDto, Choice>()
                .ForMember(dest=> dest.ID , opt => opt.MapFrom(src=> src.Id))
                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.IsCorrect, opt => opt.MapFrom(src => src.IsCorrect)).ReverseMap();
            CreateMap<Choice, ChoiceDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.IsCorrect, opt => opt.MapFrom(src => src.IsCorrect)).ReverseMap();


            // ViewModels <===> DTOS
            CreateMap<AllChoiceDto, AllChoiceViewModel>().ReverseMap();
            CreateMap<AddChoiceViewModel, AddChoiceDto>().ReverseMap();
            CreateMap<ChoiceDto, ChoiceViewModel>().ReverseMap();
            CreateMap<UpdateChoiceViewModel, UpdateChoiceDto>().ReverseMap();

        }
    }
}
