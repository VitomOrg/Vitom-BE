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
        => aspectsOf3DImplementation.Select(t => new Domain.Entities.Type
        {
            Name = t,
            Description = $"Type {t} is a wonderful type"
        }).ToArray();
}