using Foster.Framework;
using Frosty.Scripts.Core;
using System.Numerics;

namespace Frosty.Scripts.GameObjects.Effects;

public class HouseChimneySmoke : GameObject
{
    int opacity;
    float velocityY;
    float risingSpeed;

    public HouseChimneySmoke(Vector2 position, float size, float risingSpeed) : base(position, 0, Vector2.One, Vector2.One * size)
    {
        rotation = Game.Random.NextSingle() * 360;
        opacity = Game.Random.Next() % 250 + 5;
        this.risingSpeed = risingSpeed;
    }

    public override void Update()
    {
        velocityY = -risingSpeed * Time.Delta;
        rotation += Time.Delta;
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
