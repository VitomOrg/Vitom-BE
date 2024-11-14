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
    TypesResponse[] Types,
    SoftwaresResponse[] Softwares,
    IEnumerable<ImageDetailResponse> Images,
    IEnumerable<MaterialDetailResponse> ModelMaterials,
    string? FbxUrl,
    string? ObjUrl,
    string? GlbUrl,
    decimal Price,
    string DownloadUrl,
    int TotalPurchases,
    int TotalLiked,
    bool IsLiked = false
);

public record SoftwaresResponse (
    Guid Id,
    string Name
);
public record TypesResponse (
    Guid Id,
    string Name
);

