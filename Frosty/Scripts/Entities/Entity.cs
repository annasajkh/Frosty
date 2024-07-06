using Foster.Framework;
using Frosty.Scripts.Abstracts;
using Frosty.Scripts.Core;
using System.Numerics;

namespace Frosty.Scripts.Entities;

public class Entity : GameObject2D
{
    public Vector2 velocity;

    public Entity(Vector2 position) : base(position, 0, Vector2.One, Vector2.Zero)
    {

    }

    public virtual void Update()
    {
        velocity.Y += Application.gravity * Time.Delta;
        position += velocity * Time.Delta;
    }
}
