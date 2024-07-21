using System.Numerics;

namespace Frosty.Scripts.Scenes.Levels;

public class LevelN : Level
{
    public override void Startup()
    {
        base.Startup();

        player.position = new Vector2(100, 0);

        nextLevelType = NextLevelType.Bottom;

        playerAtFinishLine += () =>
        {
            GoToNextLevel("LevelO");
        };
    }
}
