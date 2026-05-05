using AutoMapper;
using Domain.Entites.CaseModule;
using Domain.Entites.PatientModule;
using Domain.Entites.StudentModule;
using Domain.Entites.TreatmentRequestModule;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Service.Abstraction;
using Service.Specifications;
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
                StudentName = student.ApplicationUser.DisplayName,
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

        public async Task<Result<RatingResponseDTO>> RateStudentAsync(string userId, CreateRatingDTO dto)
        {
            if (dto.Rating < 1 || dto.Rating > 5)
                return Error.Validation("Rating.Invalid");

            var patient = await _unitOfWork.GetRepository<Patient, int>()
                .GetByIdAsync(new PatientByUserIdSpecification(userId));

            if (patient is null)
                return Error.NotFound("Patient.NotFound");

            var request = await _unitOfWork.GetRepository<TreatmentRequest, int>()
                .GetByIdAsync(new TreatmentRequestWithDetailsSpecification(dto.TreatmentRequestId));

            if (request is null)
                return Error.NotFound("Request.NotFound");

            if (request.Case.PatientId != patient.Id)
                return Error.Unauthorized("Not.Allowed");

            if (request.Status != TreatmentRequestStatus.Accepted ||
                request.Case.Status != CaseStatus.Completed)
                return Error.Validation("Case.NotCompleted");

            // ❌ منع التكرار
            var ratings = await _unitOfWork
                .GetRepository<StudentRating, int>()
                .GetAllAsync(new RatingByRequestSpecification(request.Id));

            var hasRating = ratings.Any();

            if (hasRating)
                return Error.Validation("Rating.Exists");

            var rating = new StudentRating
            {
                TreatmentRequestId = request.Id,
                PatientId = patient.Id,
                StudentId = request.StudentId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.GetRepository<StudentRating, int>().AddAsync(rating);
            await _unitOfWork.SaveChangesAsync();

            var result = await _unitOfWork.GetRepository<StudentRating, int>()
                .GetByIdAsync(new RatingWithDetailsSpecification(rating.Id));

            return Result<RatingResponseDTO>.Ok(_mapper.Map<RatingResponseDTO>(result));
                

    }



    }
}
