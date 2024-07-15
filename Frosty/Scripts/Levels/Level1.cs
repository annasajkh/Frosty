using Foster.Framework;
using System.Numerics;

namespace Frosty.Scripts.Levels;

public class Level1 : Level
{
    public override void Startup()
    {
        base.Startup();

        transitionSpeed = 1f;
        player.position = new Vector2(30, 335);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Render(Batcher batcher)
    {
        base.Render(batcher);
    }

    public override void Shutdown()
    {
        base.Shutdown();
    }
}
