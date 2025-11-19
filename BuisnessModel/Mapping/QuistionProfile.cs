using AutoMapper;
using BuisnessModel.DTOs.Question;
using BuisnessModel.VeiwModels.Choice;
using BuisnessModel.VeiwModels.Question;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessModel.Mapping
{
    public class QuistionProfile : Profile
    {
        public QuistionProfile()
        {
            // CreateMap<Source, Destination>();
            CreateMap<Question, AllQuestionsDto>()
                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.ID))
                .ReverseMap();
            CreateMap<Question, QuestionsDto>()
                .ForMember(dest => dest.Choices, opt => opt.MapFrom(src => src.Choices))
                .ReverseMap();
            CreateMap<Question, UpdateQuestionsDto>().ReverseMap();
            CreateMap<Question, AddQuestionsDto>().ReverseMap();


            CreateMap<AllQuestionsDto, AllQuestionsViewModel>().ReverseMap();
            CreateMap<QuestionsDto, QuestionViewModel>().ReverseMap();
            CreateMap<UpdateQuestionsDto, UpdateQuestionViewModel>().ReverseMap();
            CreateMap<AddQuestionsDto, AddQuestionViewModel>().ReverseMap();
        }   
    }
}
