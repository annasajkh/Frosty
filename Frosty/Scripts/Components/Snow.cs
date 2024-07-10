using Foster.Framework;
using Frosty.Scripts.Core;
using System.Numerics;

namespace Frosty.Scripts.Components;

public class Snow
{
    public Vector2 Position { get; private set; }
    public float Size { get; }
    public float FallingSpeed { get; }
    int opacity;

    float angle;

    public Snow(Vector2 position, float size, float fallingSpeed)
    {
        Position = position;
        Size = size;
        FallingSpeed = fallingSpeed;

        angle = Game.Random.NextSingle() * 360;
        opacity = Game.Random.Next() % 250 + 5;
    }

    public void Update()
    {
        Position += Vector2.UnitY * FallingSpeed * Time.Delta;

        angle += Time.Delta;
    }

    public void Draw(Batcher batcher)
    {
        batcher.PushMatrix(Position, Vector2.One * 3, new Vector2(Size, Size) / 2, angle);
        batcher.Rect(Vector2.Zero, Vector2.One * Size, new Color(opacity, opacity, opacity, opacity));
        batcher.PopMatrix();
    }
}
