using System.Numerics;

namespace Frosty.Scripts.Scenes.Levels;

public class LevelD : Level
{
    public override void Startup()
    {
        base.Startup();

        player.position = new Vector2(30, 250);

        playerAtFinishLine += () =>
        {
            GoToNextLevel("LevelE");
        };
    }
}
