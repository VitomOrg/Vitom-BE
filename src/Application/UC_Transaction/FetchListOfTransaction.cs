using Application.Contracts;
using Application.Mappers.TransactionMappers;
using Application.Responses.Shared;
using Application.Responses.TransactionResponses;
using Ardalis.Result;
using Domain.Entities;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Transaction;

public class FetchListOfTransaction
{
    public record Query(
        bool AscByCreatedAt,
        int PageIndex,
        int PageSize
    ) : IRequest<Result<PaginatedResponse<TransactionDetailsResponse>>>;

    public class Handler(IVitomDbContext context, ICacheServices cacheServices, CurrentUser currentUser) : IRequestHandler<Query, Result<PaginatedResponse<TransactionDetailsResponse>>>
    {
        public async Task<Result<PaginatedResponse<TransactionDetailsResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            //check if user is not authenticated
            if (currentUser.User is null || currentUser.User.DeletedAt != null) return Result.Forbidden();
            //set key
            string key = $"user-transaction-pageindex{request.PageIndex}-pagesize{request.PageSize}-orderascbycreatat{request.AscByCreatedAt}-userid{currentUser.User!.Id}";
            //get cache response
            PaginatedResponse<TransactionDetailsResponse>? cacheResponse = await cacheServices.GetAsync<PaginatedResponse<TransactionDetailsResponse>>(key, cancellationToken);
            if (cacheResponse is not null) return Result.Success(cacheResponse, "Get transactions successfully!");
            //get data from db
            IQueryable<Transaction> transactions = context.Transactions
                .AsNoTracking()
                .AsSplitQuery()
                .Include(t => t.User)
                .Include(t => t.TransactionDetails)
                .ThenInclude(td => td.Product)
                .Where(t => t.UserId == currentUser.User!.Id)
                .Where(t => t.DeletedAt == null);

            //sort
            if (request.AscByCreatedAt)
            {
                transactions = transactions.OrderBy(t => t.CreatedAt);
            }
            else
            {
                transactions = transactions.OrderByDescending(t => t.CreatedAt);
            }

            //calculate total pages
            int totalPages = (int)Math.Ceiling((double)transactions.Count() / request.PageSize);

            //get result
            IEnumerable<TransactionDetailsResponse> result = await transactions
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(t => t.MapToTransactionDetailsResponse())
                .ToListAsync(cancellationToken);

            PaginatedResponse<TransactionDetailsResponse> response = new(
                result,
                request.PageIndex,
                request.PageSize,
                totalPages
            );

            //set cache
            await cacheServices.SetAsync(key, response, cancellationToken);
            return Result.Success(response, "Get transactions successfully!");
        }
    }
}
