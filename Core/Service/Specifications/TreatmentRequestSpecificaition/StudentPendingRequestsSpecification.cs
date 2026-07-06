using Domain.Entites.TreatmentRequestModule;
using Service.Specifications;

public class StudentPendingRequestsSpecification
    : BaseSpecification<TreatmentRequest, int>
{
    public StudentPendingRequestsSpecification(int studentId)
        : base(r => r.StudentId == studentId)
    {
        AddInclude(r => r.Case);
        AddInclude(r=>r.Case.Patient.ApplicationUser);
        AddOrderByDesc(r => r.CreatedAt);
    }
}