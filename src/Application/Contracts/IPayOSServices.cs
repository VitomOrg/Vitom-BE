using Net.payOS.Types;

namespace Application.Contracts;

public interface IPayOSServices
{
    Task<CreatePaymentResult> CreateOrderAsync(
        int totalAmount,
        List<ItemData> productList
    );
}