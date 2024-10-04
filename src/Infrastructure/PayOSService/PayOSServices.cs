using Application.Contracts;
using Domain.ExternalEntities;
using Microsoft.Extensions.Options;
using Net.payOS;
using Net.payOS.Types;

namespace Infrastructure.PayOSService;

public class PayOSServices(PayOS payOS, IOptions<UrlSettings> urlSettings) : IPayOSServices
{
    public async Task<CreatePaymentResult> CreateOrderAsync(int totalAmount, List<ItemData> productList)
    {

        int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
        PaymentData paymentLinkRequest =
            new(
                orderCode: orderCode,
                amount: totalAmount,
                description: $"Order {GenerateDescriptionCode()}",
                items: productList,
                returnUrl: urlSettings.Value.ReturnUrl,
                cancelUrl: urlSettings.Value.CancelUrl,
                expiredAt: (int?)DateTimeOffset.UtcNow.AddMinutes(15).ToUnixTimeSeconds()
            );

        CreatePaymentResult response = await payOS.createPaymentLink(paymentLinkRequest);
        return response;
    }
    private static int sequence = 0;
    private static readonly Random random = new();

    public static string GenerateDescriptionCode()
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        string randomPart =
            new(Enumerable.Repeat(chars, 4).Select(s => s[random.Next(s.Length)]).ToArray());

        int sequentialPart = Interlocked.Increment(ref sequence) % 10000;

        return $"{randomPart}{sequentialPart:D4}";
    }
}