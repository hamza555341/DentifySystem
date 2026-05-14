using Domain.Entites.IdentityModule;
using Domain.Entites.PatientModule;
using Domain.Entites.StudentModule;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
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
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AuthenticationService(ITokenService tokenService,
            UserManager<ApplicationUser> userManager,IUnitOfWork unitOfWork,IEmailService emailService
            ,IConfiguration configuration)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _configuration = configuration;
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

            if (!user.EmailConfirmed)
                return Error.Validation("Email.NotConfirmed", "Please confirm your email first");

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
                City = dto.City,
                UniEmail = dto.UniEmail,
                IsActive = false,
                IdentityUserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                Specializations = dto.Specializations.Aggregate((a, b) => a | b)
            };

            await _unitOfWork.GetRepository<Student, int>().AddAsync(student);
            await _unitOfWork.SaveChangesAsync();


            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var encodedToken = WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes(token));

            var baseUrl = _configuration["URLs:BaseURL"];

            var confirmLink = $"{baseUrl}api/Authentication/confirmemail" +
                              $"?userId={user.Id}&token={encodedToken}";

            var body = $@"
        <h2>Confirm Your Email</h2>
        <p>Click the link below to confirm your account:</p>
        <a href='{confirmLink}'>Confirm Email</a>";

            await _emailService.SendEmailAsync(user.Email!, "Confirm Email", body);

            return Result<UserDTO>.Ok(new UserDTO(
                user.Email!,
                user.DisplayName,
                "Check Your Email First" // مفيش token
            ));

            //var roles = await _userManager.GetRolesAsync(user);
            //var token = await _tokenService.CreateTokenAsync(
            //    user.Id, user.Email!, user.UserName!, roles);

            //return new UserDTO(user.Email!, user.DisplayName, token);
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
          
                City = dto.City,
                
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



            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var encodedToken = WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes(token));

            var baseUrl = _configuration["URLs:BaseURL"];

            var confirmLink = $"{baseUrl}api/Authentication/confirmemail" +
                              $"?userId={user.Id}&token={encodedToken}";

            var body = $@"
        <h2>Confirm Your Email</h2>
        <p>Click the link below to confirm your account:</p>
        <a href='{confirmLink}'>Confirm Email</a>";

            await _emailService.SendEmailAsync(user.Email!, "Confirm Email", body);

            return Result<UserDTO>.Ok(new UserDTO(
                user.Email!,
                user.DisplayName,
                "Check Your Email First" // مفيش token
            ));




            //var roles = await _userManager.GetRolesAsync(user);
            //var token = await _tokenService.CreateTokenAsync(
            //    user.Id, user.Email!, user.UserName!, roles);

            //return new UserDTO(user.Email!, user.DisplayName, token);
        }

        public async Task<Result> ForgetPasswordAsync(ForgotPasswordDTO dto)
        {


            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user is null)
                return Result.Ok(); // security

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var encodedToken = WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes(token));

            var baseUrl = _configuration["URLs:BaseURL"];

            var resetLink = $"{baseUrl}api/Authentication/resetpassword" +
                            $"?token={encodedToken}&email={user.Email}";

            var body = $@"
        <h2>Reset Password</h2>
        <p>Click link:</p>
        <a href='{resetLink}'>Reset</a>
        <p>If link doesn't work, use token manually:</p>
        <p>{encodedToken}</p>";

            await _emailService.SendEmailAsync(user.Email!, "Reset Password", body);

            return Result.Ok();
        }

        public async Task<Result> ResetPasswordAsync(ResetPasswordDTO dto)
        {
            if (dto.NewPassword != dto.ConfirmNewPassword)
                return Result.Failure(Error.Validation("Password.Mismatch", "Passwords do not match"));

            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user is null)
                return Result.Failure(Error.NotFound("User.NotFound"));

            string decodedToken;

            try
            {
                decodedToken = Encoding.UTF8.GetString(
                    WebEncoders.Base64UrlDecode(dto.Token));
            }
            catch
            {
                return Result.Failure(Error.Validation("Token.Invalid", "Invalid token format"));
            }

            var result = await _userManager.ResetPasswordAsync(
                user, decodedToken, dto.NewPassword);

            if (!result.Succeeded)
                return Result.Failure(result.Errors
                    .Select(e => Error.Validation(e.Code, e.Description))
                    .ToList());

            return Result.Ok();
        }

        public async Task<Result> ConfirmEmailAsync(ConfirmEmailDTO dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);

            if (user is null)
                return Result.Failure(Error.NotFound("User.NotFound"));

            if (user.EmailConfirmed)
                return Result.Failure(Error.Validation("Email.AlreadyConfirmed", "Email is already confirmed"));

            string decodedToken;

            try
            {
                decodedToken = Encoding.UTF8.GetString(
                    WebEncoders.Base64UrlDecode(dto.Token));
            }
            catch
            {
                return Result.Failure(Error.Validation("Token.Invalid", "Invalid token format"));
            }

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (!result.Succeeded)
                return Result.Failure(result.Errors
                    .Select(e => Error.Validation(e.Code, e.Description))
                    .ToList());

            return Result.Ok();
        }
    }
}



