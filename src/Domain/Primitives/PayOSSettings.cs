namespace Domain.Primitives;

public class PayOSSettings
{
    public required string ClientId { get; set; }
    public required string ApiKey { get; set; }
    public required string CheckSumKey { get; set; }
}
