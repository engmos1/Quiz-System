using BuisnessModel.DTOs.User;
using BuisnessModel.VeiwModels.User;
using DataAccess.Identity;


namespace BuisnessModel.Mapping
{
    public class UserProfile : AutoMapper.Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterViewModel, RegisterDTO>().ReverseMap();
            CreateMap<LoginViewModel, LoginDTO>().ReverseMap();
            CreateMap<ProfileViewModel, ProfileDTO>().ReverseMap();
            CreateMap<ApplicationUser, ProfileDTO>().ReverseMap();
        }
    }
}
