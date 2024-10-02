using Application.Responses.Shared;

namespace Application.Responses.ReportResponse;

public record ReportDetailResponse
(
    decimal SystemTotalIncome,
    int SystemTotalTransaction,
    int SystemTotalProduct,
    int SystemTotalUser,
    DateTimeOffset? UpdateAt,
    PaginatedResponse<MonthlyIncomeResponse> MonthlyIncomeResponses
);