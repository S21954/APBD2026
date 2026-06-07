namespace Kolokwium07062026.DTOs;

public class PaymentDto
{
    public int PaymentId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; } = null!;

    public PaymentUserDto User { get; set; } = null!;
    public PaymentCourseDto Course { get; set; } = null!;
}
