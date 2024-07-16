using System.Numerics;

namespace Frosty.Scripts.Scenes.Levels;

public class Level1 : Level
{
    public override void Startup()
    {
        base.Startup();

        transitionSpeed = 1f;
        player.position = new Vector2(30, 335);

        playerAtFinishLine += () =>
        {
            GoToNextLevel("Level2");
        };
    }
}
