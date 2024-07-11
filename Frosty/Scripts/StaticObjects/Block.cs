using Foster.Framework;
using Frosty.Scripts.Entities;
using System.Numerics;

namespace Frosty.Scripts.StaticObjects;

public class Block : Entity
{
    private static Texture blockTexture = new Texture(new Aseprite(Path.Combine("Assets", "Objects", "Block.ase")).Frames[0].Cels[0].Image);

    public Block(Vector2 position, Vector2 scale) : base(position, 0, scale, blockTexture.Size)
    {

    }

    public void Draw(Batcher batcher)
    {
        batcher.PushMatrix(position, scale, size / 2, rotation);
        batcher.Image(blockTexture, Color.White);
        batcher.PopMatrix();
    }
}
