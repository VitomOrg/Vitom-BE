using API.Utils;
using Application.Responses.ReportResponse;
using Application.UC_Report;
using Ardalis.Result;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.ReportEndpointHandler;

public class FetchReportEndpointHandler
{
    public static async Task<IResult> Handle(ISender sender,
        int? year,
        int? month,
        int pageIndex = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        Result<ReportDetailResponse> result = await sender.Send(
            new FetchReport.Query(
                Year: year,
                Month: month,
                PageIndex: pageIndex,
                PageSize: pageSize
            ), cancellationToken);

        return result.Check();
    }
}