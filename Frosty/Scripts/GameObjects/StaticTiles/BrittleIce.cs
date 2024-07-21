using Foster.Framework;
using Frosty.Scripts.Core;
using Frosty.Scripts.GameObjects.Entities;
using Frosty.Scripts.GameObjects.StaticObjects;
using Frosty.Scripts.Utils;
using System.Numerics;
using Timer = Frosty.Scripts.Components.Timer;

namespace Frosty.Scripts.GameObjects.StaticTiles;

public class BrittleIce : TileObject
{
    public bool Break { get; private set; }

    Timer breakingTimer;

    bool breakOnce;
    bool overlapGround;


    public BrittleIce(Vector2 position, Vector2 scale, Texture texture, Rect rect) : base(position, scale, texture, rect)
    {
        breakingTimer = new Timer(0.25f, true);
        breakingTimer.OnTimeout += () =>
        {
            Game.breakingSounds[Game.Random.Next() % Game.breakingSounds.Length].Play();
            Break = true;
        };

    }

    public override void ResolveCollision(Entity entity)
    {
        if (Helper.IsOverlapOnGround(entity.BoundingBox, BoundingBox))
        {
            if (!breakOnce)
            {
                breakingTimer.Start();
                breakOnce = true;
            }

            entity.friction = Game.entityOnIceFriction;
        }

        entity.ResolveAwayFrom(this);
    }

    public override void Update()
    {
        breakingTimer.Update();
    }
}