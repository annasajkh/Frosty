using Foster.Framework;
using Frosty.Scripts.Components;
using Frosty.Scripts.Core;
using System.Numerics;

namespace Frosty.Scripts.GameObjects.StaticObjects;

public class TileObject : GameObject
{
    public Texture Texture { get; }
    public Rect TextureRect { get; }
    public TileType tileType;

    public override Rect BoundingBox
    {
        get
        {
            if (tileType == TileType.Spike)
            {
                return new Rect(base.BoundingBox.X + 10, base.BoundingBox.Y + 5, base.BoundingBox.Width - 20, base.BoundingBox.Height - 10);
            }
            else
            {
                return base.BoundingBox;
            }
        }
    }

    public TileObject(Vector2 position, Vector2 scale, Texture texture, Rect rect, TileType tileType) : base(position, 0, scale, Vector2.One * Game.TileSize)
    {
        Texture = texture;
        TextureRect = rect;
        this.tileType = tileType;
    }

    public override void Draw(Batcher batcher)
    {
        batcher.PushMatrix(position, scale, size / 2, rotation);
        batcher.Image(Texture, TextureRect, Vector2.Zero, Color.White);
        batcher.PopMatrix();

        base.Draw(batcher);
    }

    public override int GetHashCode()
    {
        return new Vector2i((int)position.X, (int)position.Y).GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is TileObject other)
        {
            return new Vector2i((int)position.X, (int)position.Y).Equals(new Vector2i((int)other.position.X, (int)other.position.Y));
        }
        return false;
    }
}
