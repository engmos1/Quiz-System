using AutoMapper;
using BuisnessModel.DTOs.StudentAnswer;
using BuisnessModel.VeiwModels.StudentAnswer;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessModel.Mapping
{
    public class StudentAnswerProfile : Profile
    {
        public StudentAnswerProfile()
        {
            // CreateMap<Source, Destination>();
            CreateMap<StudentAnswerAddDto, StudentAnswer>().ReverseMap();

            CreateMap<StudentAnswerAddDto, StudentAnswerAddViewModel>().ReverseMap();
        }
    }
}
