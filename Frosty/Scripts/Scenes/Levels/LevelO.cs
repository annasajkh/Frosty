using System.Numerics;

namespace Frosty.Scripts.Scenes.Levels;

public class LevelO : Level
{
    public override void Startup()
    {
        base.Startup();

        player.position = new Vector2(170, 0);

        nextLevelType = NextLevelType.Right;

        playerAtFinishLine += () =>
        {
            GoToNextLevel("LevelP");
        };
    }
}
