using Foster.Framework;
using Frosty.Scripts.Core;
using System.Numerics;

namespace Frosty.Scripts.GameObjects.Effects;

public class Snow : GameObject
{
    public float FallingSpeed { get; }
    int opacity;

    public Snow(Vector2 position, float size, float fallingSpeed) : base(position, 0, Vector2.One, Vector2.One * size)
    {
        FallingSpeed = fallingSpeed;

        rotation = Game.Random.NextSingle() * 360;
        opacity = Game.Random.Next() % 250 + 5;
    }

    public void Update()
    {
        position += Vector2.UnitY * FallingSpeed * Time.Delta;
        rotation += Time.Delta;
    }

    public override void Draw(Batcher batcher)
    {
        batcher.PushMatrix(position, Vector2.One * Game.Scale, new Vector2(size.X, size.Y) / 2, rotation);
        batcher.Rect(Vector2.Zero, Vector2.One * size, new Color(opacity, opacity, opacity, opacity));
        batcher.PopMatrix();

        base.Draw(batcher);
    }
}
