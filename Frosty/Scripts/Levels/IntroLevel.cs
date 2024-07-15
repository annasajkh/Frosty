using Foster.Framework;
using System.Numerics;

namespace Frosty.Scripts.Levels;

public class IntroLevel : Level
{
    public override void Startup()
    {
        base.Startup();

        player.freeze = true;
        transitionSpeed = 0.5f;
        player.position = new Vector2(30, 335);

        fadeInTransitionFinished += () =>
        {
            transitionSpeed = 1f;
            dialogBox.Play(["a"]);
            //dialogBox.Play(["This day has been rough...", "I need to get back home before the snow \nstorm"]);
        };

        playerAtFinishLine += () =>
        {
            GoToNextLevel("Level1");
        };

        dialogBox.DialogFinished += () =>
        {
            player.freeze = false;
        };
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