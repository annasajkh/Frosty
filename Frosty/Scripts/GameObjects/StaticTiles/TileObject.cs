using Foster.Framework;
using Frosty.Scripts.Components;
using Frosty.Scripts.Core;
using Frosty.Scripts.GameObjects.Entities;
using System.Numerics;

namespace Frosty.Scripts.GameObjects.StaticObjects;

public class TileObject : GameObject
{
    public Texture Texture { get; }
    public Rect TextureRect { get; }

    public TileObject(Vector2 position, Vector2 scale, Texture texture, Rect rect) : base(position, 0, scale, Vector2.One * Game.TileSize)
    {
        Texture = texture;
        TextureRect = rect;
    }

    public virtual void ResolveCollision(Entity entity)
    {

    }

    public override void Update()
    {

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

    public override void Dispose()
    {
        if (!Texture.IsDisposed)
        {
            Texture.Dispose();
        }
    }
}
