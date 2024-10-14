using Application.Responses.ImageResponses;
using Application.Responses.MaterialResponses;
using Domain.Enums;

namespace Application.Responses.ProductResponses;

public record ProductDetailsResponse
(
    Guid Id,
    DateTimeOffset CreatedAt,
    string UserId,
    string License,
    string Name,
    string Description,
    IEnumerable<string> Types,
    IEnumerable<string> Softwares,
    IEnumerable<ImageDetailResponse> Images,
    IEnumerable<MaterialDetailResponse> ModelMaterials,
    string? FbxUrl,
    string? ObjUrl,
    string? GlbUrl,
    decimal Price,
    string DownloadUrl,
    int TotalPurchases,
    int TotalLiked
);