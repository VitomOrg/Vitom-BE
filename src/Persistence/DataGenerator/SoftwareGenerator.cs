using Bogus;
using Domain.Entities;

namespace Persistence.DataGenerator;

public class SoftwareGenerator
{
    readonly static string[] softwareArray = [
    "Blender",
    "Autodesk Maya",
    "Autodesk 3ds Max",
    "Cinema 4D",
    "ZBrush",
    "Houdini",
    "Rhinoceros (Rhino 3D)",
    "SketchUp",
    "MODO",
    "Marvelous Designer",
    "SolidWorks",
    "Fusion 360",
    "KeyShot",
    "Substance Painter",
    "Daz 3D",
    "Poser",
    "LightWave 3D",
    "Clara.io",
    "Tinkercad",
    "Onshape",
    "Shade 3D",
    "Art of Illusion",
    "Vectary",
    "OpenSCAD",
    "SelfCAD"
    ];
    public static Software[] Generate()
        => [.. new Faker<Software>()
            .UseSeed(1)
            .UseDateTimeReference(DateTime.UtcNow)
            // base entity
            .RuleFor(e => e.Id, f => f.Random.Uuid())
            .RuleFor(e => e.CreatedAt, f => f.Date.Past())
            .RuleFor(e => e.UpdatedAt, f => f.Random.Bool() ? f.Date.Past() : null!)
            .RuleFor(e => e.DeletedAt, (f, e) => f.Random.Bool() ? f.Date.Past() : null!)
            .RuleFor(e => e.Name, f => f.PickRandom(softwareArray))
            .RuleFor(e => e.Description, f => f.Lorem.Word())
            .RuleFor(e => e.TotalPurchases, f => f.Random.Number(1, int.MaxValue))
            .Generate(100)
            .DistinctBy(e => new {e.Name})
        ];
}