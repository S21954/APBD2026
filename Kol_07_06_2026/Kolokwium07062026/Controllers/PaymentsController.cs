using Microsoft.AspNetCore.Mvc;
using Kolokwium07062026.Services;

namespace Kolokwium07062026.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPayments(
        [FromQuery] int? userId,
        [FromQuery] string? paymentMethod)
    {
        var payments = await _paymentService.GetPaymentsAsync(userId, paymentMethod);

        return Ok(payments);
    }

}