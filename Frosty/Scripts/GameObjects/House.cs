using Foster.Framework;
using Frosty.Scripts.Core;
using System.Numerics;

namespace Frosty.Scripts.GameObjects;

public class House : GameObject
{
    static Texture houseTexture = new Texture(new Aseprite(Path.Combine("Assets", "Graphics", "Buildings", "house.ase")).Frames[0].Cels[0].Image);

    public House(Vector2 position, float rotation) : base(position, rotation, Vector2.One * Game.Scale, new Vector2(houseTexture.Width, houseTexture.Height))
    {

    }

    public override void Update()
    {

    }

    public override void Draw(Batcher batcher)
    {
        base.Draw(batcher);

        batcher.PushMatrix(position, scale, size / 2, rotation);
        batcher.Image(houseTexture, Color.White);
        batcher.PopMatrix();
    }

    public override void Dispose()
    {
        houseTexture.Dispose();
    }
}
