using Foster.Framework;
using Frosty.Scripts.Abstracts;
using Frosty.Scripts.Core;
using Frosty.Scripts.Utils;
using System.Numerics;

namespace Frosty.Scripts.Entities;

public class Entity : GameObject
{
    public float friction = 0.1f;
    public float Speed { get; private set; } = 500;

    public bool Die { get; set; }
    public bool IsOnGround { get; protected set; }
    public Vector2 velocity;
    public float CoyoteJumpDelay { get; } = 0.1f;
    public float MayJump { get; protected set; }

    private HashSet<bool> isOnGroundSet = new();

    GameObject collidingGameObject;

    public Rect CoyoteRect
    {
        get
        {
            return new Rect(position - (scale * size / 2 + new Vector2(10, 10)), position + (scale * size / 2 + new Vector2(10, 10)));
        }
    }

    public Entity(Vector2 position, float rotation, Vector2 scale, Vector2 size) : base(position, rotation, scale, size)
    {

    }

    public void ResolveAwayFrom(GameObject other)
    {
        collidingGameObject = other;

        if (Helper.IsOverlapOnGround(CoyoteRect, other.BoundingBox))
        {
            isOnGroundSet.Add(true);
        }

        if (!BoundingBox.Overlaps(other.BoundingBox))
        {
            return;
        }

        Rect collisionResult = BoundingBox.OverlapRect(other.BoundingBox);

        if (collisionResult.Width < collisionResult.Height)
        {
            if (BoundingBox.X > other.BoundingBox.X)
            {
                position.X += collisionResult.Width;

                velocity.X = 0;
                isOnGroundSet.Add(false);
            }
            else
            {
                position.X -= collisionResult.Width;

                velocity.X = 0;
                isOnGroundSet.Add(false);
            }
        }
        else
        {
            if (BoundingBox.Y > other.BoundingBox.Y)
            {
                position.Y += collisionResult.Height;

                velocity.Y = 0;
                isOnGroundSet.Add(false);
            }
            else
            {
                position.Y -= collisionResult.Height;

                velocity.Y = 0;
                isOnGroundSet.Add(true);
            }
        }
    }

    public virtual void Update()
    {
        if (isOnGroundSet.Contains(true))
        {
            IsOnGround = true;
        }
        else
        {
            friction = Game.entityFriction;
            IsOnGround = false;
        }

        isOnGroundSet.Clear();

        if (IsOnGround)
        {
            MayJump = CoyoteJumpDelay;
        }

        MayJump -= Time.Delta;

        velocity.X *= 1 - friction;
        velocity.Y += Game.gravity;
        position += velocity * Time.Delta;

        if (collidingGameObject is Entity entity)
        {            
            if (!BoundingBox.Overlaps(entity.BoundingBox))
            {
                isOnGroundSet.Add(false);
            }
        }
    }
}
