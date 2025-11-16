
using BuisnessModel.Interfaces;
using BuisnessModel.Mapping;
using BuisnessModel.Repositories;
using BuisnessModel.Services;
using BuisnessModel.Services.Auth;
using DataAccess.Context;
using DataAccess.Identity;
using DataAccess.Seed;
using ExaminationSystem.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_System
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<QuizSystemContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .LogTo(log => Debug.WriteLine(log),LogLevel.Information)
                .EnableSensitiveDataLogging(true);
            });

            builder.Services.AddScoped(typeof(IGeneralRepository<>), typeof(GeneralRepository<>));
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<IExamRepository, ExamRepository>();
            builder.Services.AddScoped<ExamService>();
            builder.Services.AddScoped<CourseService>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<UserRepository>();
            builder.Services.AddScoped<JwtTokenService>();




            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = false;
            })
             .AddEntityFrameworkStores<QuizSystemContext>()
             .AddSignInManager<SignInManager<ApplicationUser>>()   // ★ المهم جداً
             .AddDefaultTokenProviders();

            var jwt = builder.Configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwt["Key"]);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = jwt["Issuer"],
                    ValidAudience = jwt["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };
            });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddAutoMapper(typeof(CourseProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(ExamProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(ChoiceProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(QuistionProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(CourseAssignmentProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(ExamQuestionProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(StudentAnswerProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(StudentExamProfile).Assembly);



            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
               await RoleSeeder.SeedRoles(roleManager);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/openapi/v1.json", "QuizSystem API v1");

                    options.RoutePrefix = string.Empty;
                });
            }
            app.UseCors("AllowAll");

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
