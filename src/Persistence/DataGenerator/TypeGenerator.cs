using Bogus;

namespace Persistence.DataGenerator;

public class TypeGenerator
{
    readonly static string[] aspectsOf3DImplementation = [
    "Modeling",
    "Animation",
    "Sculpting",
    "Rendering",
    "Texturing",
    "Lighting",
    "Shading",
    "Rigging",
    "Simulation",
    "Visual Effects (VFX)",
    "Game Development",
    "Virtual Reality (VR)",
    "Augmented Reality (AR)",
    "Printing",
    "Computer-Aided Design (CAD)",
    "Architectural Visualization",
    "Motion Capture",
    "Character Animation",
    "Environment Design"
    ];

    public static Domain.Entities.Type[] Generate()
        => [.. new Faker<Domain.Entities.Type>()
            .UseSeed(1)
            .UseDateTimeReference(DateTime.UtcNow)
            // base entity
            .RuleFor(e => e.Id, f => f.Random.Uuid())
            .RuleFor(e => e.CreatedAt, f => f.Date.Past())
            .RuleFor(e => e.UpdatedAt, f => f.Random.Bool() ? f.Date.Past() : null!)
            .RuleFor(e => e.DeletedAt, (f, e) => f.Random.Bool() ? f.Date.Past() : null!)
            .RuleFor(e => e.Name, f => f.PickRandom(aspectsOf3DImplementation))
            .RuleFor(e => e.Description, f => f.Lorem.Word())
            .RuleFor(e => e.TotalPurchases, f => f.Random.Number(0, int.MaxValue))
            .Generate(100)
            .DistinctBy(e => e.Name)
        ];
}