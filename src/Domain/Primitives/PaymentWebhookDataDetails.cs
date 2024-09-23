namespace Domain.Primitives;

public class PaymentWebhookDataDetails
{
    public int OrderCode { get; set; }
    public int Amount { get; set; }
    public required string Description { get; set; }
    public required string AccountNumber { get; set; }
    public required string Reference { get; set; }
    public required string TransactionDateTime { get; set; }
    public required string Currency { get; set; }
    public required string PaymentLinkId { get; set; }
    public required string Code { get; set; }
    public required string Desc { get; set; }
}
