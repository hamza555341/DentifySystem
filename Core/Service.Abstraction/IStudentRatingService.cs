using Shared.CommonResult;
using Shared.DTOs.StudentRatingDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstraction
{
    public interface IStudentRatingService
    {

        Task<Result<RatingResponseDTO>> RateStudentAsync(string userId , CreateRatingDTO createRatingDTO);
        Task<Result<StudentAverageRatingDTO>> GetStudentAverageRatingAsync (int studentId);
        Task<Result<IEnumerable<RatingResponseDTO>>> GetStudentRatingsAsync(int studentId);

    }
}
