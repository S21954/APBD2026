namespace Kolokwium07062026.Services;

using Kolokwium07062026.DTOs;

public interface IPaymentService
{
    Task<IEnumerable<PaymentDto>> GetPaymentsAsync(int? userId, string? paymentMethod);
}
