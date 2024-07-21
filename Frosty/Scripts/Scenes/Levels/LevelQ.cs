using System.Numerics;

namespace Frosty.Scripts.Scenes.Levels;

public class LevelQ : Level
{
    public override void Startup()
    {
        base.Startup();

        player.position = new Vector2(30, 450);

        nextLevelType = NextLevelType.Right;

        playerAtFinishLine += () =>
        {
            GoToNextLevel("LevelR");
        };
    }
}
