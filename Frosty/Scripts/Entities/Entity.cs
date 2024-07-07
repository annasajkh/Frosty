using Foster.Framework;
using Frosty.Scripts.Abstracts;
using Frosty.Scripts.Core;
using System.Numerics;

namespace Frosty.Scripts.Entities;

public class Entity : GameObject2D
{
    public bool IsOnGround { get; protected set; }
    public Vector2 velocity;

    Entity collidingEntity;

    public Entity(Vector2 position, float rotation, Vector2 scale, Vector2 size) : base(position, rotation, scale, size)
    {

    }

    public void ResolveAwayFrom(Entity other)
    {
        collidingEntity = other;

        if (!Rect.Overlaps(other.Rect))
        {
            return;
        }

        Rect collisionResult = Rect.OverlapRect(other.Rect);

        if (collisionResult.Width < collisionResult.Height)
        {
            if (Rect.X > other.Rect.X)
            {
                position.X += collisionResult.Width;

                velocity.X = 0;
                IsOnGround = false;
            }
            else
            {
                position.X -= collisionResult.Width;

                velocity.X = 0;
                IsOnGround = false;
            }
        }
        else
        {
            if (Rect.Y > other.Rect.Y)
            {
                position.Y += collisionResult.Height;

                velocity.Y = 0;
                IsOnGround = false;
            }
            else
            {
                position.Y -= collisionResult.Height;

                velocity.Y = 0;
                IsOnGround = true;
            }
        }
    }

    public virtual void Update()
    {
        velocity.Y += Application.gravity;
        position += velocity * Time.Delta;

        if (collidingEntity is Entity entity)
        {            
            if (!Rect.Overlaps(entity.Rect))
            {
                IsOnGround = false;
            }
        }
    }
}
