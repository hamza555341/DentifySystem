using AutoMapper;
using Domain.Entites.CaseModule;
using Domain.Entites.PatientModule;
using Domain.Entites.StudentModule;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Service.Abstraction;
using Service.Specifications.CaseSpecifications;
using Service.Specifications.StudentRatingSpecifications;
using Shared.CommonResult;
using Shared.DTOs.StudentRatingDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Service
{
    public class StudentRatingService:IStudentRatingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StudentRatingService(IUnitOfWork unitOfWork,IMapper mapper)
        {
           _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<Result<StudentAverageRatingDTO>> GetStudentAverageRatingAsync(int studentId)
        {
            var student = await _unitOfWork.GetRepository<Student, int>().
                GetByIdAsync(studentId);
            if (student is null)
                return Error.NotFound("Student.NotFound");

            var Ratings = await _unitOfWork.GetRepository<StudentRating,int>()
                .GetAllAsync(new RatingsByStudentSpecification(studentId));

            var RatingList= Ratings.ToList();

            var Average = RatingList.Any() ? 
                Math.Round(RatingList.Average(r => r.Rating), 1) :0.0 ;

            return new StudentAverageRatingDTO
            {
                StudentId = studentId,
                StudentName = student.FullName,
                AverageRating = Average,
                TotalRatings = RatingList.Count()

            };

        }

        public async Task<Result<IEnumerable<RatingResponseDTO>>> GetStudentRatingsAsync(int studentId)
        {
            var student = await _unitOfWork.GetRepository<Student, int>()
             .GetByIdAsync(studentId);

            if (student is null)
                return Error.NotFound("Student.NotFound");

            var ratings = await _unitOfWork.GetRepository<StudentRating, int>()
                .GetAllAsync(new RatingsByStudentSpecification(studentId));

            return Result<IEnumerable<RatingResponseDTO>>.Ok(
                _mapper.Map<IEnumerable<RatingResponseDTO>>(ratings));
        }

        public async Task<Result<RatingResponseDTO>> RateStudentAsync(string userId, CreateRatingDTO createRatingDTO)
        {
            if (createRatingDTO.Rating < 1 || createRatingDTO.Rating > 5)
                return Error.Validation("Rating.Invalid", "Rating must be between 1 and 5");

            var patient= await _unitOfWork.GetRepository<Patient,int>()
                .GetByIdAsync(new PatientByUserIdSpecification(userId));

            if (patient is null)
                return Error.NotFound("Patient.NotFound", "Patient not found for the given user ID");


            var caseEntity = await _unitOfWork.GetRepository<Case, int>()
                .GetByIdAsync(createRatingDTO.CaseId);

             if (caseEntity is null)
                return Error.NotFound("Case.NotFound", "Case not found for the given case ID");


             if (caseEntity.PatientId != patient.Id)
                return Error.Unauthorized("Unauthorized", "You are not authorized to rate this student");

            if (caseEntity.Status != CaseStatus.Completed)
                return Error.Validation("Case.NotCompleted", "You can only rate a student for completed cases");

            if (caseEntity.AssignedStudentId is null)
                return Error.Validation("Case.NotAssigned", "No student assigned to this case");

            var studentId = caseEntity.AssignedStudentId.Value;

            var student = await _unitOfWork.GetRepository<Student, int>()
                .GetByIdAsync(studentId);

            if (student is null)
                return Error.NotFound("Student.NotFound");




            var ExsitingRating = await _unitOfWork.GetRepository<StudentRating, int>()
                                           .GetByIdAsync(new RatingByCaseAndPatientSpecification(createRatingDTO.CaseId, patient.Id));


            if(ExsitingRating is not null)
                return Error.Validation("Rating.Exists", "You have already rated this student for this case");

            var RatingEntity = new StudentRating
            {
                CaseId = createRatingDTO.CaseId,
                PatientId = patient.Id,
                StudentId = studentId,
                Rating = createRatingDTO.Rating,
                Comment = createRatingDTO.Comment,
                CreatedAt = DateTime.UtcNow

            };

         await _unitOfWork.GetRepository<StudentRating, int>().AddAsync(RatingEntity);
         await _unitOfWork.SaveChangesAsync();

            var RatingResult = await _unitOfWork.GetRepository<StudentRating, int>()
                .GetByIdAsync(new RatingByCaseAndPatientSpecification(createRatingDTO.CaseId, patient.Id));


            var responseDTO = _mapper.Map<RatingResponseDTO>(RatingResult);

            return Result<RatingResponseDTO>.Ok(responseDTO);


        }



    }
}
