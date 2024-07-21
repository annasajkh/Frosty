using Foster.Framework;
using Frosty.Scripts.Core;
using System.Numerics;

namespace Frosty.Scripts.GameObjects.Effects;

public class BrittleIceBreak : GameObject
{
    int opacity;
    float velocityY;
    float maxVelocityY = 300;

    public BrittleIceBreak(Vector2 position, float size) : base(position, 0, Vector2.One, Vector2.One * size)
    {
        rotation = Game.Random.NextSingle() * 360;
        opacity = Game.Random.Next() % 250 + 5;
    }

    public override void Update()
    {
        velocityY += Game.gravity;
        velocityY = Math.Clamp(velocityY, -maxVelocityY, maxVelocityY);
        position += Vector2.UnitY * velocityY * Time.Delta;
    }

    public override void Draw(Batcher batcher)
    {
        batcher.PushMatrix(position, Vector2.One * Game.Scale, new Vector2(size.X, size.Y) / 2, rotation);
        batcher.Rect(Vector2.Zero, Vector2.One * size, new Color(opacity, opacity, opacity, opacity));
        batcher.PopMatrix();

        base.Draw(batcher);
    }

    public override void Dispose()
    {

    }
}