using Foster.Framework;
using Frosty.Scripts.Abstracts;
using Frosty.Scripts.Core;
using Frosty.Scripts.Entities;
using System.Numerics;

namespace Frosty.Scripts.StaticObjects;

public class Block : StaticObject
{
    public Block(Vector2 position, Vector2 scale) : base(position)
    {
        base.scale = scale;
        size = Application.blockTexture.Size;
    }

    public void Draw(Batcher batcher)
    {
        batcher.PushMatrix(position, scale, size / 2, rotation);
        batcher.Image(Application.blockTexture, Color.White);
        batcher.PopMatrix();
    }

    public void ResolveAwayFrom(GameObject2D other)
    {
        if (!Rect.Overlaps(other.Rect))
        {
            return;
        }

        Rect collisionResult = Rect.OverlapRect(other.Rect);

        if (collisionResult.Width < collisionResult.Height)
        {
            if (Rect.X > other.Rect.X)
            {
                other.position.X -= collisionResult.Width;
            }
            else
            {
                other.position.X += collisionResult.Width;
            }
        }
        else
        {
            if (Rect.Y > other.Rect.Y)
            {
                other.position.Y -= collisionResult.Height;

                if (other is Entity entity)
                {
                    entity.velocity.Y = 0;
                }
            }
            else
            {
                other.position.Y += collisionResult.Height;
            }
        }
    }
}
