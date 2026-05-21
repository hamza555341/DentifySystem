using AutoMapper;
using Domain.Entites.IdentityModule;
using Domain.Entites.PatientModule;
using Domain.Entites.StudentModule;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Service.Abstraction;
using Service.Specifications.CaseSpecifications;
using Shared.CommonResult;
using Shared.DTOs.ProfileDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<ProfileResponseDTO>> GetCurrentProfileAsync(
            string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return Error.NotFound("User.NotFound");

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Patient"))
            {
                var patient = await _unitOfWork
                    .GetRepository<Patient, int>()
                    .GetByIdAsync(
                        new PatientByUserIdSpecification(userId));

                if (patient is null)
                    return Error.NotFound("Patient.NotFound");

                return Result<ProfileResponseDTO>.Ok(
                    _mapper.Map<ProfileResponseDTO>(patient));
            }

            var student = await _unitOfWork
                .GetRepository<Student, int>()
                .GetByIdAsync(
                    new StudentByUserIdSpecification(userId));

            if (student is null)
                return Error.NotFound("Student.NotFound");

            return Result<ProfileResponseDTO>.Ok(
                _mapper.Map<ProfileResponseDTO>(student));
        }

        public async Task<Result> UpdateProfileAsync(
      string userId,
      UpdateProfileDTO dto)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return Error.NotFound("User.NotFound");

            user.DisplayName = dto.FullName;
            user.PhoneNumber = dto.PhoneNumber;

            var identityResult =
                await _userManager.UpdateAsync(user);

            if (!identityResult.Succeeded)
                return identityResult.Errors
                    .Select(e =>
                        Error.Validation(
                            e.Code,
                            e.Description))
                    .ToList();

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Student"))
            {
                var student = await _unitOfWork
                    .GetRepository<Student, int>()
                    .GetByIdAsync(
                        new StudentByUserIdSpecification(userId));

                if (student is null)
                    return Error.NotFound("Student.NotFound");

                if (dto.Specializations is null ||
                    !dto.Specializations.Any())
                {
                    return Error.Validation(
                        "Specializations.Required",
                        "At least one specialization is required");
                }

                student.Specializations =
                    dto.Specializations.Aggregate(
                        Specialization.None,
                        (current, next) => current | next);

                _unitOfWork
                    .GetRepository<Student, int>()
                    .Update(student);
            }
            else
            {
                var patient = await _unitOfWork
                    .GetRepository<Patient, int>()
                    .GetByIdAsync(
                        new PatientByUserIdSpecification(userId));

                if (patient is null)
                    return Error.NotFound("Patient.NotFound");
            }

            await _unitOfWork.SaveChangesAsync();

            return Result.Ok();
        }

        public async Task<Result> ChangePasswordAsync(
            string userId,
            ChangePasswordDTO dto)
        {
            if (dto.NewPassword != dto.ConfirmPassword)
                return Error.Validation(
                    "Password.Mismatch",
                    "Passwords do not match");

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return Error.NotFound("User.NotFound");

            var result =
                await _userManager.ChangePasswordAsync(
                    user,
                    dto.CurrentPassword,
                    dto.NewPassword);

            if (!result.Succeeded)
                return result.Errors
                    .Select(e =>
                        Error.Validation(
                            e.Code,
                            e.Description))
                    .ToList();

            return Result.Ok();
        }
    }
}
