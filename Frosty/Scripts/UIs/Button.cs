using Foster.Framework;
using Frosty.Scripts.GameObjects;
using System.Numerics;

namespace Frosty.Scripts.UIs;

public class Button : GameObject
{
    public Button(Vector2 position, Vector2 size) : base(position, 0, Vector2.One, size)
    {

    }

    public override void Update()
    {
        
    }

    public override void Draw(Batcher batcher)
    {


        base.Draw(batcher);
    }

    public override void Dispose()
    {

    }
}
