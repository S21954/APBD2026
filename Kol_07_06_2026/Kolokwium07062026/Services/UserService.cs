using Kolokwium07062026.Data;
using Microsoft.EntityFrameworkCore;

namespace Kolokwium07062026.Services;


public class UserService : IUserService
{
    private readonly PaymentsDbContext _context;

    public UserService(PaymentsDbContext context)
    {
        _context = context;
    }

    public async Task<DeleteUserResult> DeleteUserAsync(int id)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        var user = await _context.Users.FindAsync(id);
        if (user is null)
        {
            return DeleteUserResult.NotFound;
        }

        var hasRelations =
            await _context.Payments.AnyAsync(p => p.UserId == id) ||
            await _context.Enrollments.AnyAsync(e => e.UserId == id) ||
            await _context.Reviews.AnyAsync(r => r.UserId == id) ||
            await _context.Submissions.AnyAsync(s => s.UserId == id) ||
            await _context.Certificates.AnyAsync(c => c.UserId == id) ||
            await _context.Courses.AnyAsync(c => c.InstructorId == id);

        if (hasRelations)
        {
            return DeleteUserResult.HasRelatedData;
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        await transaction.CommitAsync();

        return DeleteUserResult.Deleted;
    }
}
