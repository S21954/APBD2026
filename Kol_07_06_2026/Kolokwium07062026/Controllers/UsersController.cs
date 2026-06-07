using Microsoft.AspNetCore.Mvc;
using Kolokwium07062026.Services;

namespace Kolokwium07062026.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var result = await _userService.DeleteUserAsync(id);

        return result switch
        {
            DeleteUserResult.Deleted => NoContent(),

            DeleteUserResult.NotFound => NotFound(),

            DeleteUserResult.HasRelatedData => Conflict(
                "Cannot delete user because related data exists."),

            _ => StatusCode(500)
        };
    }
}