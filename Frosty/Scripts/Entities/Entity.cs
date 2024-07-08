using Foster.Framework;
using Frosty.Scripts.Abstracts;
using Frosty.Scripts.Core;
using System.Numerics;
using Timer = Frosty.Scripts.Components.Timer;

namespace Frosty.Scripts.Entities;

public class Entity : GameObject
{
    public bool IsOnGroundCoyote { get; protected set; }
    public bool IsOnGround { get; protected set; }
    public Vector2 velocity;

    Entity collidingEntity;

    Timer coyoteJumpTimer;

    public Rect RectCoyote
    {
        get
        {
            return new Rect(position - (scale * size / 2 + new Vector2(10, 10)), position + (scale * size / 2 + new Vector2(10, 10)));
        }
    }

    public Entity(Vector2 position, float rotation, Vector2 scale, Vector2 size) : base(position, rotation, scale, size)
    {
        coyoteJumpTimer = new Timer(0.5f, true);

        coyoteJumpTimer.OnTimeout += () =>
        {
            IsOnGroundCoyote = false;
        };
    }

    public void ResolveAwayFrom(Entity other)
    {
        collidingEntity = other;


        if (RectCoyote.Overlaps(other.Rect))
        {
            Rect collisionCoyoteResult = RectCoyote.OverlapRect(other.Rect);

            if (collisionCoyoteResult.Width > collisionCoyoteResult.Height && RectCoyote.Y < other.Rect.Y)
            {
                IsOnGroundCoyote = true;
                IsOnGround = true; 
            }
        }

        if (!Rect.Overlaps(other.Rect))
        {
            return;
        }

        Rect collisionResult = Rect.OverlapRect(other.Rect);

        if (collisionResult.Width < collisionResult.Height)
        {
            float differentY = MathF.Abs(Rect.Y - other.Rect.Y);

            if (Rect.X > other.Rect.X)
            {
                position.X += collisionResult.Width;

                velocity.X = 0;
                coyoteJumpTimer.Start();
                IsOnGround = false;
            }
            else
            {
                position.X -= collisionResult.Width;

                velocity.X = 0;
                coyoteJumpTimer.Start();
                IsOnGround = false;
            }
        }
        else
        {
            if (Rect.Y > other.Rect.Y)
            {
                position.Y += collisionResult.Height;

                velocity.Y = 0;
                coyoteJumpTimer.Start();
                IsOnGround = false;
            }
            else
            {
                position.Y -= collisionResult.Height;

                velocity.Y = 0;
                IsOnGroundCoyote = true;
                IsOnGround = true;
            }
        }
    }

    public virtual void Update()
    {
        coyoteJumpTimer.Update();

        velocity.Y += Application.gravity;
        position += velocity * Time.Delta;

        if (collidingEntity is Entity entity)
        {            
            if (!Rect.Overlaps(entity.Rect))
            {
                coyoteJumpTimer.Start();
                IsOnGround = false;
            }
        }
    }
}
