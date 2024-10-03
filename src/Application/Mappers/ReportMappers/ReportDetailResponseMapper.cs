using Application.Responses.ReportResponse;
using Domain.Entities.Report;

namespace Application.Mappers.ReportMappers;

public static class ReportDetailResponseMapper
{
    public static ReportDetailResponse MapToReportResponse(SystemTotal systemTotal, IEnumerable<MonthlyIncome> monthlyIncome, int PageIndex, int PageSize)
     => new(
        SystemTotalIncome: systemTotal.TotalIncome,
        SystemTotalTransaction: systemTotal.TotalTransactions,
        SystemTotalProduct: systemTotal.TotalProducts,
        SystemTotalUser: systemTotal.TotalUsers,
        UpdateAt: systemTotal.UpdatedAt,
        MonthlyIncomeResponses: new(
            Data: monthlyIncome.Select(mi => mi.MapToMonthlyIncomeResponse()),
            PageIndex: PageIndex,
            PageSize: PageSize,
            TotalPages: (int)Math.Ceiling((Double)monthlyIncome.Count() / PageSize)
        )
     );
}