using AutoMapper;
using BuisnessModel.DTOs.User;
using BuisnessModel.Services.Auth;
using BuisnessModel.VeiwModels.User;
using DataAccess.Models.Enums;
using ExaminationSystem.Attributes;
using ExaminationSystem.Models;
using ExaminationSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace ExaminationSystem.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _distributedCache;


        public AuthController(AuthService authService, IMapper mapper, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            _authService = authService;
            _mapper = mapper;
            _memoryCache = memoryCache;
            _distributedCache = distributedCache;
        }

        [HttpPost]
        public async Task<ResponseViewModel<bool>> Register(RegisterViewModel vm)
        {
            var dto = _mapper.Map<RegisterDTO>(vm);

            var (success, message) = await _authService.Register(dto);

            if (!success)
                return ResponseViewModel<bool>.Failure(message, ErrorCode.ValidationFailed);

            return ResponseViewModel<bool>.Success(true);
        }

        [HttpPost]
        public async Task<ResponseViewModel<object>> Login(LoginViewModel vm)
        {
            var dto = _mapper.Map<LoginDTO>(vm);

            var response = await _authService.Login(dto);

            if (!response.Success)
                return ResponseViewModel<object>.Failure(response.Message, ErrorCode.ValidationFailed);

            return ResponseViewModel<object>.Success(new
            {
                token = response.Token,
                profile = response.Profile
            });
        }

        [HttpGet]
        [Benchmark]
        public async Task<ResponseViewModel<ProfileViewModel>> Profile(string userId)
        {
            var profile = await _authService.GetProfile(userId);

            if (profile == null)
                return ResponseViewModel<ProfileViewModel>.Failure("User not found", ErrorCode.UserNotFound);

            return ResponseViewModel<ProfileViewModel>.Success(_mapper.Map<ProfileViewModel>(profile));
        }
        [HttpGet]
        [Benchmark]
        public async Task<ResponseViewModel<ProfileViewModel>> ProfileCache(string userId)
        {
            if (!_memoryCache.TryGetValue($"profile_{userId}", out ProfileDTO profile))
            {
                profile = await _authService.GetProfile(userId);

                if (profile == null)
                    return ResponseViewModel<ProfileViewModel>.Failure("User not found", ErrorCode.UserNotFound);

                _memoryCache.Set($"profile_{userId}", profile, TimeSpan.FromMinutes(10));
            }

            return ResponseViewModel<ProfileViewModel>.Success(_mapper.Map<ProfileViewModel>(profile));
        }


        [HttpGet]
        [Benchmark]
        public async Task<ResponseViewModel<ProfileViewModel>> ProfileRedis(string userId)
        {
            var cacheKey = $"profile_{userId}";
            ProfileDTO profile;

            // Try get from Redis
            var cachedData = await _distributedCache.GetStringAsync(cacheKey);

            if (cachedData != null)
            {
                profile = JsonSerializer.Deserialize<ProfileDTO>(cachedData);
            }
            else
            {
                profile = await _authService.GetProfile(userId);

                if (profile == null)
                    return ResponseViewModel<ProfileViewModel>.Failure("User not found", ErrorCode.UserNotFound);

                var serialized = JsonSerializer.Serialize(profile);

                await _distributedCache.SetStringAsync(
                    cacheKey,
                    serialized,
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    }
                );
            }

            return ResponseViewModel<ProfileViewModel>.Success(_mapper.Map<ProfileViewModel>(profile));
        }
        [HttpPost]
        public async Task<ResponseViewModel<bool>> UpdateProfile(ProfileViewModel vm)
        {
            var dto = _mapper.Map<ProfileDTO>(vm);

            var success = await _authService.UpdateProfile(dto);

            if (!success)
                return ResponseViewModel<bool>.Failure("Update failed", ErrorCode.ValidationFailed);

            return ResponseViewModel<bool>.Success(true);
        }

        [HttpPost]
        public async Task<ResponseViewModel<bool>> ChangePassword(string userId, string oldPassword, string newPassword)
        {
            var success = await _authService.ChangePassword(userId, oldPassword, newPassword);

            if (!success)
                return ResponseViewModel<bool>.Failure("Password change failed", ErrorCode.ValidationFailed);

            return ResponseViewModel<bool>.Success(true);
        }
    }
}
