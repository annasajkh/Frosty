using Foster.Framework;
using Frosty.Scripts.Core;
using Frosty.Scripts.Entities;
using System.Numerics;

namespace Frosty.Scripts.StaticObjects;

public class Block : Entity
{
    public Block(Vector2 position, Vector2 scale) : base(position, 0, scale, Application.blockTexture.Size)
    {

    }

    public void Draw(Batcher batcher)
    {
        batcher.PushMatrix(position, scale, size / 2, rotation);
        batcher.Image(Application.blockTexture, Color.White);
        batcher.PopMatrix();
    }
}
