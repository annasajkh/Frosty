using Foster.Framework;
using Frosty.Scripts.Core;
using Frosty.Scripts.GameObjects.Effects;
using Frosty.Scripts.Utils;
using System.Numerics;

namespace Frosty.Scripts.Scenes;

public class MainMenu : Scene
{
    protected SnowSpawner snowSpawner;


    public override void Startup()
    {
        snowSpawner = new SnowSpawner(new Vector2(0, 0), 0.005f, App.Width);
    }

    public override void Update()
    {
        if (Input.Keyboard.Pressed(Keys.Enter))
        {
            Game.SceneManager.ChangeScene("IntroLevel");
        }

        snowSpawner.Update();
    }

    public override void Render(Batcher batcher)
    {
        Graphics.Clear(Color.CornflowerBlue);

        batcher.Image(Game.NightSky, Vector2.Zero, Color.White);
        snowSpawner.Draw(batcher);

        Helper.DrawTextCentered("Press Enter to Start", new Vector2(App.Width / 2, App.Height / 2), Color.White, Game.M5x7Menu, batcher);
    }

    public override void Shutdown()
    {

    }
}
