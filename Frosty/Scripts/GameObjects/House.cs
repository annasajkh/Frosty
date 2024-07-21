using Foster.Framework;
using Frosty.Scripts.Core;
using System.Numerics;

namespace Frosty.Scripts.GameObjects;

public class House : GameObject
{
    public House(Vector2 position, float rotation) : base(position, rotation, Vector2.One * Game.Scale, new Vector2(Game.houseTexture.Width, Game.houseTexture.Height))
    {

    }

    public override void Update()
    {

    }

    public override void Draw(Batcher batcher)
    {
        base.Draw(batcher);

        batcher.PushMatrix(position, scale, size / 2, rotation);
        batcher.Image(Game.houseTexture, Color.White);
        batcher.PopMatrix();
    }

    public override void Dispose()
    {

    }
}
