using API.Utils;
using Application.Responses.Shared;
using Application.Responses.TransactionResponses;
using Application.UC_Transaction;
using Ardalis.Result;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.TransactionEndpointHandlers;

public class FetchListOfTransactionEndpointHandler
{
    public static async Task<IResult> Handle(ISender sender,
        bool ascByCreatedAt = false,
        int pageIndex = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        Result<PaginatedResponse<TransactionDetailsResponse>> result = await sender.Send(new FetchListOfTransaction.Query(
            AscByCreatedAt: ascByCreatedAt,
            PageIndex: pageIndex,
            PageSize: pageSize
        ), cancellationToken);
        return result.Check();
    }
}