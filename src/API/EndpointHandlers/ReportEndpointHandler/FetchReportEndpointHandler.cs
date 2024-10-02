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
        int? Year,
        int? Month,
        int PageIndex = 1,
        int PageSize = 10,
        CancellationToken cancellationToken = default)
    {
        Result<ReportDetailResponse> result = await sender.Send(
            new FetchReport.Query(
                Year: Year,
                Month: Month,
                PageIndex: PageIndex,
                PageSize: PageSize
            ), cancellationToken);

        return result.Check();
    }
}