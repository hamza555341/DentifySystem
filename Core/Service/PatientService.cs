using AutoMapper;
using Domain.Entites.CaseModule;
using Domain.Entites.PatientModule;
using Domain.Entites.StudentModule;
using Domain.Interfaces;
using Service.Abstraction;
using Service.Specifications.CaseSpecifications;
using Service.Specifications.StudentSpecification;
using Shared.CommonResult;
using Shared.DTOs.StudentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PatientService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<IEnumerable<StudentResponseDTO>>> GetAvailableStudentsAsync(int caseId, string identityUserId)
        {
            var patient = await _unitOfWork.GetRepository<Patient, int>()
                .GetByIdAsync(new PatientByUserIdSpecification(identityUserId));

            if (patient is null)
                return Error.NotFound("Patient.NotFound");

            var case_ = await _unitOfWork.GetRepository<Case, int>()
                .GetByIdAsync(new CaseWithImagesSpecification(caseId));

            if (case_ is null)
                return Error.NotFound("Case.NotFound");

            if (case_.PatientId != patient.Id)
                return Error.Unauthorized("Case.Unauthorized");

            var students = await _unitOfWork.GetRepository<Student, int>()
                .GetAllAsync(new AvailableStudentsByCaseSpecification(case_.RequiredSpecialization, case_?.City));

            var result = _mapper.Map<IEnumerable<StudentResponseDTO>>(students);

            return Result<IEnumerable<StudentResponseDTO>>.Ok(result);
        }
    }
}
