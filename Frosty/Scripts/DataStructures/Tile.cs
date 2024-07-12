using Foster.Framework;
using System.Numerics;

namespace Frosty.Scripts.DataStructures;

[Serializable]
public struct Tile
{
    public int x;
    public int y;

    public int rectX;
    public int rectY;
    public int rectWidth;
    public int rectHeight;

    public Tile(Vector2 position, Rect rect)
    {
        x = (int)position.X;
        y = (int)position.Y;

        rectX = (int)rect.X;
        rectY = (int)rect.Y;
        rectWidth = (int)rect.Width;
        rectHeight = (int)rect.Height;
    }

    public override int GetHashCode()
    {
        return new Vector2i((int)x, (int)y).GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is Tile other)
        {
            return new Vector2i((int)x, (int)y).Equals(new Vector2i((int)other.x, (int)other.y));
        }
        return false;
    }
}
