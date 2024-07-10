using Foster.Framework;
using Frosty.Scripts.Entities;
using Frosty.Scripts.Scenes;
using System.Numerics;

namespace Frosty.Scripts.StaticObjects;

public class Block : Entity
{
    public Block(Vector2 position, Vector2 scale) : base(position, 0, scale, TestLevel.blockTexture.Size)
    {

    }

    public void Draw(Batcher batcher)
    {
        batcher.PushMatrix(position, scale, size / 2, rotation);
        batcher.Image(TestLevel.blockTexture, Color.White);
        batcher.PopMatrix();
    }
}
