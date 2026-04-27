using Shared.CommonResult;
using Shared.DTOs.IdentityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstraction
{
    public interface IAuthenticationService
    {
        // Login
        //Email ,Password => Token ,Email ,DisplayName
        Task<Result<UserDTO>> LoginAsyns(LoginDTO loginDto);

        //Register Patient
        //Email, Password , UserName ,PhoneNumber ,DisplayName => Token ,Email ,DisplayName
        Task<Result<UserDTO>> RegisterPatientAsync(RegisterPatientDTO dto);

        //Register Doctor
        // Email, Password , UserName ,PhoneNumber ,DisplayName,Bio,Fees,SpecializationId => Token ,Email ,DisplayName
        Task<Result<UserDTO>> RegisterStudentAsync(RegisterStudentDTO dto);

        Task<Result<CurrentUserDTO>> GetCurrentUserAsync(string userId);

        Task<bool> CheckEmailAsync(string email);

    }

}



