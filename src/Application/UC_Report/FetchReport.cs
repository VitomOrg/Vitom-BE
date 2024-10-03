using Application.Contracts;
using Application.Mappers.ReportMappers;
using Application.Responses.ReportResponse;
using Ardalis.Result;
using Domain.Entities.Report;
using Domain.Primitives;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Report;

public class FetchReport
{
    public record Query(
        int? Year,
        int? Month,
        int PageIndex,
        int PageSize
    ) : IRequest<Result<ReportDetailResponse>>;

    public class Handler(IVitomDbContext context, ICacheServices cacheServices, CurrentUser currentUser) : IRequestHandler<Query, Result<ReportDetailResponse>>
    {
        public async Task<Result<ReportDetailResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            //check if user is not admin
            if (!currentUser.User!.IsAdmin() || currentUser.User.DeletedAt != null) return Result.Forbidden();
            //set key
            var cacheKey = $"Report_{request.Year}_{request.Month}";
            //get data from cache
            ReportDetailResponse? cachedData = await cacheServices.GetAsync<ReportDetailResponse>(cacheKey, cancellationToken);
            if (cachedData is not null)
                return Result.Success(cachedData, "Get report successfully");
            //get system total data
            var systemTotal = await context.SystemTotals
                .FirstOrDefaultAsync(cancellationToken);
            //get users
            var users = await context.Users
                .AsNoTracking()
                .Where(u => u.DeletedAt == null)
                .ToListAsync(cancellationToken);
            //get products
            var products = await context.Products
                .AsNoTracking()
                .Where(p => p.DeletedAt == null)
                .ToListAsync(cancellationToken);
            //get transactions
            var transactions = await context.Transactions
                .AsNoTracking()
                .Where(t => t.DeletedAt == null)
                .ToListAsync(cancellationToken);
            //HANLDE SYSTEM TOTAL
            //check if system total is null or not
            if (systemTotal is null)
            {
                //add new system total report if there is no total report data
                systemTotal = new()
                {
                    TotalIncome = transactions.Sum(t => t.TotalAmount),
                    TotalTransactions = transactions.Count,
                    TotalProducts = products.Count,
                    TotalUsers = users.Count,
                };
                await context.SystemTotals.AddAsync(systemTotal, cancellationToken);
            }
            else
            {
                systemTotal.Update(transactions.Sum(t => t.TotalAmount), transactions.Count, products.Count, users.Count);
            }
            //HANDLE MONTHLY INCOME
            //check if request month and year is not null
            List<MonthlyIncome> monthIncomes = [];
            if (request.Month is null && request.Year is null)
            {
                monthIncomes = await context.MonthlyIncomes.AsNoTracking().ToListAsync(cancellationToken);
            }
            else if (request.Month is not null && request.Year is not null)
            {
                //get statistics for month and year
                var specificMonthIncomes = await context.MonthlyIncomes
                    .Where(m => m.Month == request.Month && m.Year == request.Year)
                    .SingleOrDefaultAsync(cancellationToken);
                //calculate total income and total transaction for specific month
                //check if specific month income is null or not
                if (specificMonthIncomes is null)
                {
                    //add new month income report if there is no month income report data
                    specificMonthIncomes = new()
                    {
                        Year = request.Year.Value,
                        Month = request.Month.Value,
                        TotalIncome = transactions
                            .Where(t => t.CreatedAt.Year == request.Year && t.CreatedAt.Month == request.Month)
                            .Sum(t => t.TotalAmount),
                        TotalTransaction = transactions
                            .Where(t => t.CreatedAt.Year == request.Year && t.CreatedAt.Month == request.Month)
                            .Count(),
                    };
                    await context.MonthlyIncomes.AddAsync(specificMonthIncomes, cancellationToken);
                }
                else if (specificMonthIncomes is not null)
                {
                    // update the specific month income report if it existed
                    var updateTotalIncome = transactions
                        .Where(t => t.CreatedAt.Year == request.Year && t.CreatedAt.Month == request.Month)
                        .Sum(t => t.TotalAmount);
                    var updateTotalTransaction = transactions
                        .Where(t => t.CreatedAt.Year == request.Year && t.CreatedAt.Month == request.Month)
                        .Count();
                    specificMonthIncomes.Update(updateTotalIncome, updateTotalTransaction);
                }
                monthIncomes.Add(specificMonthIncomes!);
            }
            else
            {
                return Result.Error("input must have both month and year or none of them");
            }
            //save changes
            await context.SaveChangesAsync(cancellationToken);
            //map to response
            ReportDetailResponse cacheResponse = ReportDetailResponseMapper.MapToReportResponse(systemTotal, monthIncomes, request.PageIndex, request.PageSize);
            //cache data
            await cacheServices.SetAsync(cacheKey, cacheResponse, cancellationToken);
            return Result.Success(cacheResponse, "Get report successfully");
        }
    }

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;

            RuleFor(x => x.Year)
                .GreaterThan(0).WithMessage("Year must be greater than 0")
                .LessThanOrEqualTo(currentYear).WithMessage("Year cannot be greater than the current year");

            RuleFor(x => x.Month)
                .GreaterThan(0).WithMessage("Month must be greater than 0")
                .LessThanOrEqualTo(12).WithMessage("Month must be between 1 and 12")
                .Must((query, month) =>
                    query.Month is null
                    || query.Year < currentYear
                    || (query.Year == currentYear && month <= currentMonth)
                ).WithMessage("Month cannot be greater than the current month in the current year");

            RuleFor(x => x.PageIndex)
                .GreaterThan(0).WithMessage("Page index must be greater than 0");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0");
        }
    }
}