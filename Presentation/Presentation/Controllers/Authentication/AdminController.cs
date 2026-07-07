using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers;
using Service.Abstraction;

[Authorize(Roles = "Admin")]
public class AdminController : ApiBaseController
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet("students")]
    public async Task<IActionResult> GetPendingStudents()
    {
        var result = await _adminService.GetPendingStudentsAsync();
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
    }


    [HttpPut("students/{studentId}/reject")]
    public async Task<IActionResult> RejectStudent(int studentId)
    {
        var result = await _adminService.RejectStudentAsync(studentId);
        return HandleResult(result);
    }
}