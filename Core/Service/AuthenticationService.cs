using Domain.Entites.IdentityModule;
using Domain.Entites.PatientModule;
using Domain.Entites.StudentModule;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Service.Abstraction;
using Shared.CommonResult;
using Shared.DTOs.IdentityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public AuthenticationService(ITokenService tokenService,
            UserManager<ApplicationUser> userManager,IUnitOfWork unitOfWork)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CheckEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }

        public async Task<Result<CurrentUserDTO>> GetCurrentUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return Error.NotFound("User.NotFound");


            var roles = await _userManager.GetRolesAsync(user);

            return new CurrentUserDTO(user.Email!, user.DisplayName, roles.FirstOrDefault()!);         
        }

        public async Task<Result<UserDTO>> LoginAsyns(LoginDTO loginDto)
        {

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null)
                return Error.InValidCerdentials("User.InvalidCredentials");

            var isPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!isPassword)
                return Error.InValidCerdentials("User.InvalidCredentials");

            //if (!user.EmailConfirmed)
            //    return Error.Validation("Email.NotConfirmed", "Please confirm your email first");

            var roles = await _userManager.GetRolesAsync(user);

            //if (roles.Contains("Student"))
            //{
            //    var student = await _unitOfWork.GetRepository<Student, int>()
            //   .GetAllAsync();

            //    var approvedStudent = student.FirstOrDefault(s => s.IdentityUserId == user.Id);

            //    if (approvedStudent is null || !approvedStudent.IsApproved)
            //        return Error.Validation("Student.NotApproved",
            //            "Your account is pending admin approval");
            //}

            var token = await _tokenService.CreateTokenAsync(
                user.Id, user.Email!, user.UserName!, roles);

            return new UserDTO(user.Email!, user.DisplayName, token);

        }

        public async Task<Result<UserDTO>> RegisterStudentAsync(RegisterStudentDTO dto)
        {
            if (await _userManager.FindByEmailAsync(dto.Email) is not null)
                return Error.Validation("Email.Exists", "Email already exists");

            var user = new ApplicationUser
            {
                UserName = dto.UserName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                DisplayName = dto.FullName,
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return result.Errors
                    .Select(e => Error.Validation(e.Code, e.Description))
                    .ToList();

            await _userManager.AddToRoleAsync(user, "Student");

            var student = new Student
            {
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                Email= dto.Email,
                University = dto.University,
                AcademicYear = dto.AcademicYear,
                IsApproved = false,
                IdentityUserId = user.Id,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.GetRepository<Student, int>().AddAsync(student);
            await _unitOfWork.SaveChangesAsync();

            var roles = await _userManager.GetRolesAsync(user);
            var token = await _tokenService.CreateTokenAsync(
                user.Id, user.Email!, user.UserName!, roles);

            return new UserDTO(user.Email!, user.DisplayName, token);
        }


        public async Task<Result<UserDTO>> RegisterPatientAsync(RegisterPatientDTO dto)
        {
            if (await _userManager.FindByEmailAsync(dto.Email) is not null)
                return Error.Validation("Email.Exists", "Email already exists");

            var user = new ApplicationUser
            {
                UserName = dto.UserName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                DisplayName = dto.FullName,
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return result.Errors
                    .Select(e => Error.Validation(e.Code, e.Description))
                    .ToList();

            await _userManager.AddToRoleAsync(user, "Patient");

            var patient = new Patient
            {
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                City = dto.City,
                Governorate = dto.Governorate,
                NationalId = dto.NationalId,
                ChronicDiseases = dto.ChronicDiseases,
                IdentityUserId = user.Id,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await _unitOfWork.GetRepository<Patient, int>().AddAsync(patient);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = await _tokenService.CreateTokenAsync(
                user.Id, user.Email!, user.UserName!, roles);

            return new UserDTO(user.Email!, user.DisplayName, token);
        }


    }
}



