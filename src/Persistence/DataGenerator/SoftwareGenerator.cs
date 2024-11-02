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
        => softwareArray.Select(s => new Software
        {
            Name = s,
            Description = $"Software {s} is a wonderful software"
        }).ToArray();
}