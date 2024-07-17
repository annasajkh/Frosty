using Foster.Framework;
using Frosty.Scripts.Core;
using Frosty.Scripts.GameObjects.Entities;
using Frosty.Scripts.GameObjects.StaticObjects;
using Frosty.Scripts.Utils;
using System.Numerics;

namespace Frosty.Scripts.GameObjects.StaticTiles;

public class Ice : TileObject
{
    public Ice(Vector2 position, Vector2 scale, Texture texture, Rect rect) : base(position, scale, texture, rect)
    {

    }

    public override void ResolveCollision(Entity entity)
    {
        if (Helper.IsOverlapOnGround(entity.BoundingBox, BoundingBox))
        {
            entity.friction = Game.entityOnIceFriction;
        }

        entity.ResolveAwayFrom(this);
    }
}
