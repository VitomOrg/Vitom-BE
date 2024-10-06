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
    IEnumerable<string> ImageUrls,
    IEnumerable<string> ModelMaterialUrls,
    string? FbxUrl,
    string? ObjUrl,
    string? GlbUrl,
    decimal Price,
    string DownloadUrl,
    int TotalPurchases,
    int TotalLiked
);
