using Foster.Framework;
using Frosty.Scripts.Abstracts;
using Frosty.Scripts.Core;
using Frosty.Scripts.Utils;
using System.Numerics;

namespace Frosty.Scripts.Scenes;

public class MainMenu : Scene
{
    public override void Startup()
    {

    }

    public override void Update()
    {
        if (Input.Keyboard.Pressed(Keys.Enter))
        {
            Game.SceneManager.ChangeScene("TestLevel");
        }
    }

    public override void Render(Batcher batcher)
    {
        Graphics.Clear(Color.CornflowerBlue);

        Helper.DrawTextCentered("Press Enter to Start", new Vector2(App.Width / 2, App.Height / 2), Color.White, Game.ArialFont, batcher);
    }

    public override void Shutdown()
    {

    }
}
