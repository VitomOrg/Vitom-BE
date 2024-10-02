using Application.Responses.ReportResponse;
using Domain.Entities.Report;

namespace Application.Mappers.ReportMappers;

public static class MonthlyIncomeResponseMapper
{
    public static MonthlyIncomeResponse MapToMonthlyIncomeResponse(this MonthlyIncome monthlyIncome)
        => new(
            Year: monthlyIncome.Year,
            Month: monthlyIncome.Month,
            TotalIncome: monthlyIncome.TotalIncome,
            TotalTransaction: monthlyIncome.TotalTransaction,
            CreatedAt: monthlyIncome.CreatedAt,
            UpdatedAt: monthlyIncome.UpdatedAt
        );
}