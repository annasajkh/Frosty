using Foster.Framework;
using Frosty.Scripts.Abstracts;
using Frosty.Scripts.Core;
using Frosty.Scripts.DataStructures;
using System.Numerics;

namespace Frosty.Scripts.StaticObjects;

public class TileObject : GameObject
{
    public Texture Texture { get; }
    public Rect TextureRect { get; }
    public TileType tileType;

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

        if (tileType == TileType.Spike)
        {
            if (Game.DebugMode)
            {
                Rect spikeRect = new Rect(Rect.X + 10, Rect.Y + 5, Rect.Width - 20, Rect.Height - 10);

                batcher.RectLine(spikeRect, 1, Color.Green);
                return;
            }
        }

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
