using System.Numerics;

namespace Frosty.Scripts.Scenes.Levels;

public class LevelC : Level
{
    public override void Startup()
    {
        base.Startup();

        player.position = new Vector2(30, 250);

        playerAtFinishLine += () =>
        {
            GoToNextLevel("LevelD");
        };
    }
}