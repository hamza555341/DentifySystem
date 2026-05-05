using Domain.Entites.AppointmentModule;
using Domain.Entites.CaseModule;
using Domain.Entites.ChatModule;
using Domain.Entites.ReportModule;
using Domain.Entites.StudentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites.TreatmentRequestModule
{
    public class TreatmentRequest : BaseEntity<int>
    {
        public int CaseId { get; set; }
        public int StudentId { get; set; }
        public TreatmentRequestStatus Status { get; set; } = TreatmentRequestStatus.Pending;
        public Case Case { get; set; } = null!;
        public Student Student { get; set; } = null!;
        public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
        public ICollection<Appointment> Appointments { get; set; }

        // ✅ Report (One)
        public Report? Report { get; set; }

        // ✅ Rating (One)
        public StudentRating? Rating { get; set; }

        public RequestInitiator InitiatedBy { get; set; }

    }
}
