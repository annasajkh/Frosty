using System.Numerics;

namespace Frosty.Scripts.Scenes.Levels;

public class LevelM : Level
{
    public override void Startup()
    {
        base.Startup();

        player.position = new Vector2(250, 450);

        nextLevelType = NextLevelType.Bottom;

        playerAtFinishLine += () =>
        {
            GoToNextLevel("LevelN");
        };
    }
}
