using Microsoft.AspNetCore.Mvc;
using Notes.WebAPI.CustomActionFilters;
using Notes.WebAPI.Repositories;

namespace Notes.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly IDashboardRepository _dashboardRepository;

    public DashboardController(IDashboardRepository dashboardRepository)
    {
        _dashboardRepository = dashboardRepository;
    }

    [HttpGet("users")]
    [ValidateModel]
    public async Task<ActionResult<ApiResponse>> GetUsers()
    {
        var response = await _dashboardRepository.GetAllUsersAsync();

        if (response.Success)
        {
            return Ok(response);
        }
        
        return BadRequest(response);
    }

    [HttpDelete("{userId}")]
    public async Task<ActionResult<ApiResponse>> DeleteUser([FromRoute] string userId)
    {
        var response = await _dashboardRepository.DeleteUserAsync(userId);

        if (response.Success)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }
}
