using System.Numerics;

namespace Frosty.Scripts.Scenes.Levels;

public class Level4 : Level
{
    public override void Startup()
    {
        base.Startup();

        player.position = new Vector2(30, 250);
    }
}
