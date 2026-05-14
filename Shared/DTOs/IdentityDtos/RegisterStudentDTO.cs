using Domain.Entites.StudentModule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.IdentityDtos
{
    public record RegisterStudentDTO(
      string UserName,
     [EmailAddress] string Email,
      string Password,
      string FullName,
     [Phone] string PhoneNumber,
      string City,

      string UniEmail,

      List<Specialization> Specializations
  );
}
