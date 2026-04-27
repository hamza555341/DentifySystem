using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.AppointmentDtos
{
    public class AppointmentResponseDTO
    {
      public int Id { get; set; }
      public int CaseId { get; set; }
      public string PatientName { get; set; } = null!;
      public string StudentName { get; set; }=  null!;
      public string  Location { get; set; } =null!;
      public string  Status { get; set; }= null!;
      public DateTimeOffset AppointmentDate { get; set; }


    }
}
