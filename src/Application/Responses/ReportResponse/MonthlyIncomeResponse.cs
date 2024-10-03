namespace Application.Responses.ReportResponse;

public record MonthlyIncomeResponse
(
    int Year,
    int Month,
    decimal TotalIncome,
    decimal TotalTransaction,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
);