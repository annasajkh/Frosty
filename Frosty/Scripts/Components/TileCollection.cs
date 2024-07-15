using Foster.Framework;
using Frosty.Scripts.DataStructures;

namespace Frosty.Scripts.Components;


/// <summary>
/// Collection of tiles
/// </summary>
public class TileCollection
{
    public Texture Texture { get; }
    public string[] AsepritePath { get; }

    public List<Tile> Tiles { get; } = new();

    public TileCollection(string[] asepritePath, List<Tile> tiles)
    {
        Texture = new Texture(new Aseprite(Path.Combine(asepritePath)).Frames[0].Cels[0].Image);
        AsepritePath = asepritePath;
        Tiles = tiles;
    }
}