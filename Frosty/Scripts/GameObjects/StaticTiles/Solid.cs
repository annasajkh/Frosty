using Foster.Framework;
using Frosty.Scripts.Core;
using Frosty.Scripts.GameObjects.Entities;
using Frosty.Scripts.GameObjects.StaticObjects;
using Frosty.Scripts.Utils;
using System.Numerics;

namespace Frosty.Scripts.GameObjects.StaticTiles;

public class Solid : TileObject
{
    public Solid(Vector2 position, Vector2 scale, Texture texture, Rect rect) : base(position, scale, texture, rect)
    {
    }

    public override void ResolveCollision(Entity entity)
    {
        if (Helper.IsOverlapOnGround(entity.BoundingBox, entity.BoundingBox))
        {
            entity.friction = Game.entityFriction;
        }

        entity.ResolveAwayFrom(this);
    }
}
