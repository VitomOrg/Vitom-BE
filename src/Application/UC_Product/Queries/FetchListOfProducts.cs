﻿using Application.Contracts;
using Application.Mappers.ProductMappers;
using Application.Responses.ProductResponses;
using Application.Responses.Shared;
using Ardalis.Result;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace Application.UC_Product.Queries
{
    public class FetchListOfProducts
    {
        public record Query(
            string? Search,
            decimal PriceFrom,
            decimal PriceTo,
            Guid[] SoftwareIds,
            bool AscByCreatedAt,
            int PageSize,
            int PageIndex,
            Guid[] TypeIds,
            LicenseEnum? License
        ) : IRequest<Result<PaginatedResponse<ProductDetailsResponse>>>;



        public class Handler(IVitomDbContext context, ICacheServices cacheServices) : IRequestHandler<Query, Result<PaginatedResponse<ProductDetailsResponse>>>
        {
            public async Task<Result<PaginatedResponse<ProductDetailsResponse>>> Handle(Query request, CancellationToken cancellationToken)
            {
                //set key
                string key = $"products-productpage-pagesize{request.PageSize}-pageindex{request.PageIndex}-sortascbycreateddate{request.AscByCreatedAt}-types{string.Join(",", request.TypeIds)}-license{request.License}-pricefrom{request.PriceFrom}-priceto{request.PriceTo}-search{request.Search}-softwares{string.Join(",", request.SoftwareIds)}";
                //get cache response
                PaginatedResponse<ProductDetailsResponse>? cacheResponse = await cacheServices.GetAsync<PaginatedResponse<ProductDetailsResponse>>(key, cancellationToken);
                if (cacheResponse is not null) return Result.Success(cacheResponse, "Get products successfully!");
                //query
                IQueryable<Product> query = context.Products
                .AsNoTracking()
                .AsSplitQuery()
                .Include(p => p.ProductTypes).ThenInclude(p => p.Type)
                .Include(p => p.ProductSoftwares).ThenInclude(p => p.Software)
                .Include(p => p.ProductImages)
                .Include(p => p.ModelMaterials)
                .Include(p => p.Model)
                .Where(p => p.DeletedAt == null)
                .Where(p => request.Search == null || p.Name.ToLower().Contains(request.Search.ToLower()))
                .Where(p => request.SoftwareIds.Length == 0 || p.ProductSoftwares.Any(ps => request.SoftwareIds.Contains(ps.SoftwareId)))
                .Where(p => request.TypeIds.Length == 0 || p.ProductTypes.Any(ps => request.TypeIds.Contains(ps.TypeId)))
                .Where(p => p.License == request.License || request.License == null)
                .Where(p => p.Price >= request.PriceFrom && p.Price < request.PriceTo);
                //sort
                if (request.AscByCreatedAt)
                {
                    query = query.OrderBy(p => p.CreatedAt);
                }
                else
                {
                    query = query.OrderByDescending(p => p.CreatedAt);
                }
                //calculate total pages
                int totalPages = (int)Math.Ceiling((double)query.Count() / request.PageSize);
                //get result
                IEnumerable<ProductDetailsResponse> result = await query
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(p => p.MapToProductDetailsResponse())
                    .ToListAsync(cancellationToken);
                //map to paginated response
                cacheResponse = new(
                    Data: result,
                    PageIndex: request.PageIndex,
                    PageSize: request.PageSize,
                    TotalPages: totalPages
                );
                //set to cache
                await cacheServices.SetAsync(key, cacheResponse, cancellationToken);
                //return result
                return Result.Success(cacheResponse, "Get products successfully!");
            }
        }
    }

    
}