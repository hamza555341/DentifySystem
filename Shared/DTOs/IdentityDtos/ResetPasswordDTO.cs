using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.IdentityDtos
{
    public record ResetPasswordDTO(
     string Email,
     string Token,
     string NewPassword,
     string ConfirmNewPassword);
}
