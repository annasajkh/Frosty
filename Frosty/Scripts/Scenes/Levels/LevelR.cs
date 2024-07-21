using Foster.Framework;
using Frosty.Scripts.Core;
using Frosty.Scripts.GameObjects;
using Frosty.Scripts.GameObjects.Effects;
using Frosty.Scripts.Utils;
using System.Numerics;

namespace Frosty.Scripts.Scenes.Levels;

public class LevelR : Level
{
    bool showEndUI;
    House house;
    HouseChimneySmokeSpawner houseChimneySmokeSpawner;

    public override void Startup()
    {
        base.Startup();
        dialogBox = new DialogBox(new Vector2(App.Width / 2, 21 * Game.Scale + 5), 10);

        stopGameRuntime = true;
        player.freeze = true;
        fadeInTransitionFinished += () =>
        {
            dialogBox.Play(["home sweet home", "and there is no snow storm yet", "finally i can rest from all of this madness"]);
        };

        dialogBox.DialogFinished += () =>
        {
            transitionSpeed = 0.25f;
            fadeOut = true;
            player.walkRight = true;
            player.speed = 50;
        };

        fadeOutTransitionFinished += () =>
        {
            player.walkRight = false;
            showEndUI = true;
        };


        player.position = new Vector2(30, 450);
        house = new House(new Vector2(650, 306), 0);
        houseChimneySmokeSpawner = new HouseChimneySmokeSpawner(new Vector2(312, 150), 0.1f, 30);
    }

    public override void Update()
    {
        base.Update();

        house.Update();
        houseChimneySmokeSpawner.Update();
    }

    public override void DrawHouse(Batcher batcher)
    {
        houseChimneySmokeSpawner.Draw(batcher);
        house.Draw(batcher);
    }

    public override void Render(Batcher batcher)
    {
        base.Render(batcher);

        if (showEndUI)
        {
            Helper.DrawTextCentered($"Game Run Time: {TimeSpan.FromSeconds(Game.GameRunTime).ToString(@"hh\:mm\:ss")}", new Vector2(App.Width / 2, App.Height / 4), Color.White, Game.M5x7Menu, batcher);
            Helper.DrawTextCentered($"Deaths: {Game.PlayerDeath}", new Vector2(App.Width / 2, App.Height / 4 * 2), Color.White, Game.M5x7Menu, batcher);
        }
    }

    public override void Shutdown()
    {
        base.Shutdown();
        house.Dispose();
    }
}
