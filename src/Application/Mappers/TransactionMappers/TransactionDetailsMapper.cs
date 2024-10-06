using Application.Responses.TransactionResponses;
using Domain.Enums;
using Transaction = Domain.Entities.Transaction;

namespace Application.Mappers.TransactionMappers;

public static class TransactionDetailsMapper
{
    public static TransactionDetailsResponse MapToTransactionDetailsResponse(this Transaction transaction)
    {
        return new(
            UserId: transaction.UserId,
            TotalAmount: transaction.TotalAmount,
            PaymentMethod: Enum.TryParse(transaction.PaymentMethod.ToString(), out PaymentMethodEnum result) ? result.ToString() : "unknown payment method",
            TransactionStatus: Enum.TryParse(transaction.TransactionStatus.ToString(), out TransactionStatusEnum status) ? status.ToString() : "unknown transaction status",
            CreatedAt: transaction.CreatedAt,
            ProductImage: transaction.TransactionDetails.FirstOrDefault()?.Product.ProductImages.FirstOrDefault()?.Url ?? string.Empty
        );
    }
}
