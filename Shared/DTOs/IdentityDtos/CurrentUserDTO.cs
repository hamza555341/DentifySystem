using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.IdentityDtos
{
    public record CurrentUserDTO(string UserId,string Email, string DisplayName,string role);
   
}
