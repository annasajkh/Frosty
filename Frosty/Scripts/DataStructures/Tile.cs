using Foster.Framework;
using System.Numerics;

namespace Frosty.Scripts.DataStructures;

public struct Tile
{
    public Vector2 Position { get; }
    public Rect Rect { get; }

    public Tile(Vector2 position, Rect rect)
    {
        Position = position;
        Rect = rect;
    }
}
