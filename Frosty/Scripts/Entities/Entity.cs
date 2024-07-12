using Foster.Framework;
using Frosty.Scripts.Abstracts;
using Frosty.Scripts.Core;
using System.Numerics;

namespace Frosty.Scripts.Entities;

public class Entity : GameObject
{
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

        if (CoyoteRect.Overlaps(other.Rect))
        {
            Rect collisionCoyoteResult = CoyoteRect.OverlapRect(other.Rect);

            if (collisionCoyoteResult.Width > collisionCoyoteResult.Height && CoyoteRect.Y < other.Rect.Y)
            {
                isOnGroundSet.Add(true); 
            }
        }

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
            if (Rect.Y > other.Rect.Y)
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
            IsOnGround = false;
        }

        isOnGroundSet.Clear();

        if (IsOnGround)
        {
            MayJump = CoyoteJumpDelay;
        }

        MayJump -= Time.Delta;

        velocity.Y += Game.gravity;
        position += velocity * Time.Delta;

        if (collidingGameObject is Entity entity)
        {            
            if (!Rect.Overlaps(entity.Rect))
            {

                isOnGroundSet.Add(false);
            }
        }
    }
}
