using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstraction;
using Shared.DTOs.StudentRatingDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class RatingController : ApiBaseController
    {
        private readonly IStudentRatingService _studentRatingService;

        public RatingController(IStudentRatingService studentRatingService)
        {
            _studentRatingService = studentRatingService;
        }

        [HttpPost]
        [Authorize(Roles = "Patient")]

        public async Task<ActionResult<RatingResponseDTO>> RateStudent(CreateRatingDTO dto)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return HandleResult( await _studentRatingService.RateStudentAsync(UserId!,dto));
        
        }
       

        [HttpGet("student/{studentId}/average")]
        [Authorize]
        public async Task<ActionResult<StudentAverageRatingDTO>> GetStudentAverage(
            int studentId)
        {
            return HandleResult(
                await _studentRatingService.GetStudentAverageRatingAsync(studentId));
        }

        [HttpGet("student/{studentId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<RatingResponseDTO>>> GetStudentRatings(
            int studentId)
        {
            return HandleResult(
                await _studentRatingService.GetStudentRatingsAsync(studentId));
        }






    }
}
