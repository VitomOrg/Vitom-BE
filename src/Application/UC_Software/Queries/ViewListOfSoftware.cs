using Application.Contracts;
using Application.Mappers.SoftwareMappers;
using Application.Responses.Shared;
using Application.Responses.SoftwareResponses;
using Ardalis.Result;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Software.Queries;

public class ViewListOfSoftware
{
    public record Query(
        string Keyword,
        int PageSize,
        int PageIndex
    ) : IRequest<Result<PaginatedResponse<SoftwareDetailsResponse>>>;
    public class Handler(IVitomDbContext context, ICacheServices cacheServices) : IRequestHandler<Query, Result<PaginatedResponse<SoftwareDetailsResponse>>>
    {
        public async Task<Result<PaginatedResponse<SoftwareDetailsResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            // set key
            string key = $"software-keyword{request.Keyword}-pagesize{request.PageSize}-pageindex-{request.PageIndex}";
            // get cache result
            PaginatedResponse<SoftwareDetailsResponse>? cacheResult = await cacheServices.GetAsync<PaginatedResponse<SoftwareDetailsResponse>>(key, cancellationToken);
            if (cacheResult is not null) return Result.Success(cacheResult, "Get softwares Successfully");
            // query
            IQueryable<Software> query =
                context.Softwares
                    .AsNoTracking()
                    .Where(s => s.Name.ToLower().Contains(request.Keyword.ToLower()))
                    .Where(s => s.DeletedAt != null);
            // calculating total pages
            int totalPages = (int)Math.Ceiling((decimal)query.Count() / request.PageSize);
            // get result
            IEnumerable<SoftwareDetailsResponse> result =
                await query.Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(s => s.MapToSoftwareDetailsResponse())
                    .ToListAsync(cancellationToken);
            // map to paginated result
            cacheResult = new(
                Data: result,
                PageIndex: request.PageIndex,
                PageSize: request.PageSize,
                TotalPages: totalPages
            );
            // set to cache
            await cacheServices.SetAsync(key, cacheResult, cancellationToken);
            // return result
            return Result.Success(cacheResult, "Get softwares successfully !");
        }
    }
}
