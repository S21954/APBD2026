namespace Kolokwium07062026.Services;


public interface IUserService
{
    Task<DeleteUserResult> DeleteUserAsync(int id);
}
