namespace Domain.Primitives;

public class PaymentWebhookData
{
    public required string Code { get; set; }
    public required string Desc { get; set; }
    public required bool Success { get; set; }
    public required PaymentWebhookDataDetails Data { get; set; }
    public required string Signature { get; set; }
}