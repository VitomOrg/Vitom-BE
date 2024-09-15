using Domain.Enums;

namespace Application.Responses.TransactionResponses;

public record TransactionDetailsResponse(
    string UserId,
    decimal TotalAmount,
    string PaymentMethod,
    string TransactionStatus
);
