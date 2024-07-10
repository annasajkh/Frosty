using Foster.Framework;
using Frosty.Scripts.Abstracts;
using Frosty.Scripts.Core;
using System.Numerics;

namespace Frosty.Scripts.Scenes;

public class MainMenu : Scene
{
    SpriteFont font;

    string title = "Press Enter to Start";

    public override void Startup()
    {
        font = new SpriteFont(Path.Combine("Assets", "Fonts", "Arial.ttf"), 32);
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

        batcher.PushMatrix(new Vector2(App.Width / 2, App.Height / 2), Vector2.One, new Vector2(font.WidthOf(title) / 2, font.HeightOf(title) / 2), 0);
        batcher.Text(font, title, Vector2.Zero, Color.White);
        batcher.PopMatrix();

    }

    public override void Shutdown()
    {

    }
}
