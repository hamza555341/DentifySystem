using Shared.CommonResult;
using Shared.DTOs.ProfileDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstraction
{
    public interface IAccountService
    {
        Task<Result<ProfileResponseDTO>> GetCurrentProfileAsync(string userId);

        Task<Result> UpdateProfileAsync(
            string userId,
            UpdateProfileDTO dto);

        Task<Result> ChangePasswordAsync(
            string userId,
            ChangePasswordDTO dto);
    }
}
