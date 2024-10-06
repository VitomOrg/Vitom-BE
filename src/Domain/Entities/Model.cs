using Domain.Primitives;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Model : Entity
{
    public Guid ProductId { get; set; }
    [RegularExpression(@".*\.fbx$", ErrorMessage = "The file must end with .fbx.")]
    public required string Fbx { get; set; }
    [RegularExpression(@".*\.obj$", ErrorMessage = "The file must end with .obj.")]
    public required string Obj { get; set; }
    [RegularExpression(@".*\.glb$", ErrorMessage = "The file must end with .glb.")]
    public required string Glb { get; set; }
    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; } = null!;
    public void Update(string fbx, string obj, string glb)
    {
        Fbx = fbx;
        Obj = obj;
        Glb = glb;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}