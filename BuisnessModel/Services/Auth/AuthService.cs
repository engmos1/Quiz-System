using AutoMapper;
using BuisnessModel.DTOs.User;
using BuisnessModel.Repositories;
using DataAccess.Identity;
using Microsoft.AspNetCore.Identity;


namespace BuisnessModel.Services.Auth
{
    public class AuthService
    {
        private readonly UserRepository _userRepo;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtTokenService _jwtService;
        private readonly IMapper _mapper;

        public AuthService(
            UserRepository userRepo,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            JwtTokenService jwtService,
            IMapper mapper,
            RoleManager<IdentityRole> roleManager)
        {
            _userRepo = userRepo;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public async Task<(bool Success, string Message)> Register(RegisterDTO dto)
        {
            var existing = await _userManager.FindByEmailAsync(dto.Email);
            if (existing != null)
                return (false, "Email already exists");

            var user = new ApplicationUser
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserName = dto.Email,
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return (false, "Registration failed");

            // Add role
            await _userManager.AddToRoleAsync(user, dto.Role);

            return (true, "Registered successfully");
        }
 
        public async Task<(bool Success, string Message, string Token, ProfileDTO Profile)> Login(LoginDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return (false, "Invalid email or password", null, null);

            var check = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!check.Succeeded)
                return (false, "Invalid email or password", null, null);

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            var token = _jwtService.GenerateToken(user, role);

            var profile = _mapper.Map<ProfileDTO>(user);
            profile.Role = role;

            return (true, "Login successful", token, profile);
        }

 
        public async Task<ProfileDTO?> GetProfile(string userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            var dto = _mapper.Map<ProfileDTO>(user);
            dto.Role = roles.FirstOrDefault();

            return dto;
        }
 
        public async Task<bool> UpdateProfile(ProfileDTO dto)
        {
            var user = await _userRepo.GetByIdAsync(dto.Id);
            if (user == null)
                return false;

            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.UserName = dto.Email;

            if (!string.IsNullOrEmpty(dto.Role))
            {
                if (!await _roleManager.RoleExistsAsync(dto.Role))
                    return false;

                var oldRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, oldRoles);

                var roleResult = await _userManager.AddToRoleAsync(user, dto.Role);
                if (!roleResult.Succeeded)
                    return false;
            }

            return await _userRepo.UpdateAsync(user);
        }

        
        public async Task<bool> ChangePassword(string userId, string oldPass, string newPass)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                return false;

            return await _userRepo.ChangePasswordAsync(user, oldPass, newPass);
        }
    }
}
