using Kolokwium07062026.DTOs;
using Kolokwium07062026.Data;
using Kolokwium07062026.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kolokwium07062026.Services;

public class PaymentService : IPaymentService
{
    private readonly PaymentsDbContext _context;

    public PaymentService(PaymentsDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PaymentDto>> GetPaymentsAsync(int? userId, string? paymentMethod)
    {
        var query = _context.Payments
            .AsNoTracking()
            .AsQueryable();

        if (userId.HasValue)
        {
            query = query.Where(p => p.UserId == userId.Value);
        }

        if (!string.IsNullOrWhiteSpace(paymentMethod))
        {
            query = query.Where(p => p.PaymentMethod == paymentMethod);
        }

        return await query
            .Select(p => new PaymentDto
            {
                PaymentId = p.Id,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                PaymentMethod = p.PaymentMethod,

                User = new PaymentUserDto
                {
                    Id = p.User.Id,
                    FirstName = p.User.FirstName,
                    LastName = p.User.LastName
                },

                Course = new PaymentCourseDto
                {
                    Id = p.Course.Id,
                    Title = p.Course.Title
                }
            })
            .ToListAsync();
    }
}