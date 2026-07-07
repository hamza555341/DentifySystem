using AutoMapper;
using Domain.Entites.StudentModule;
using Domain.Interfaces;
using Service.Abstraction;
using Service.Specifications.AdminSpecification;
using Shared.CommonResult;
using Shared.DTOs.StudentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AdminService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<StudentResponseDTO>>> GetPendingStudentsAsync()
        {
            var students = await _unitOfWork.GetRepository<Student, int>()
                .GetAllAsync(new PendingStudentsSpecification());

            var result = _mapper.Map<IEnumerable<StudentResponseDTO>>(students);
            return Result<IEnumerable<StudentResponseDTO>>.Ok(result);
        }


        public async Task<Result> RejectStudentAsync(int studentId)
        {
            var student = await _unitOfWork.GetRepository<Student, int>()
                .GetByIdAsync(studentId);

            if (student is null)
                return Error.NotFound("Student.NotFound");

            student.IsActive = false;
            _unitOfWork.GetRepository<Student, int>().Update(student);
            await _unitOfWork.SaveChangesAsync();

            return Result.Ok();
        }
    }
}
