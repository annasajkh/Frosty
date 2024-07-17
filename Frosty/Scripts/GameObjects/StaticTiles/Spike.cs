using Foster.Framework;
using Frosty.Scripts.GameObjects.Entities;
using Frosty.Scripts.GameObjects.StaticObjects;
using System.Numerics;

namespace Frosty.Scripts.GameObjects.StaticTiles;

public class Spike : TileObject
{

    public override Rect BoundingBox
    {
        get
        {
            return new Rect(base.BoundingBox.X + 10, base.BoundingBox.Y + 5, base.BoundingBox.Width - 20, base.BoundingBox.Height - 10);
        }
    }

    public Spike(Vector2 position, Vector2 scale, Texture texture, Rect rect) : base(position, scale, texture, rect)
    {

    }

    public override void ResolveCollision(Entity entity)
    {
        if (entity.BoundingBox.Overlaps(this.BoundingBox))
        {
            entity.Die = true;
        }
    }
}
