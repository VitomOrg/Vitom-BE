using Domain.Enums;

namespace Application.Responses.TransactionResponses;

public class TransactionDetailsResponse(
    string UserId,
    decimal TotalAmount,
    string PaymentMethod,
    string TransactionStatus
);
