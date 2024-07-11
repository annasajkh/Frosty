using Foster.Framework;
using Frosty.Scripts.Abstracts;
using Frosty.Scripts.Components;
using Frosty.Scripts.Core;
using Frosty.Scripts.Entities;
using Frosty.Scripts.StaticObjects;
using System.Numerics;

namespace Frosty.Scripts.Scenes;

public class TestLevel : Scene
{
    public static Texture nightSky;

    List<Block> blocks;
    Player player;

    Snowing snowing;

    public override void Startup()
    {
        snowing = new Snowing(new Vector2(0, 0), 0.005f, App.Width);

        nightSky = new Texture(new Aseprite(Path.Combine("Assets", "Backgrounds", "night_sky.ase")).Frames[0].Cels[0].Image);

        blocks = new List<Block>();
        player = new Player(new Vector2(100, 100));

        for (int i = 0; i < 20; i++)
        {
            blocks.Add(new Block(new Vector2(i * 80, 500), new Vector2(3, 3)));
        }
    }

    public override void Update()
    {
        if (Input.Keyboard.Pressed(Keys.Escape))
        {
            Game.SceneManager.ChangeScene("MainMenu");
        }

        player.Update();

        foreach (var block in blocks)
        {
            player.ResolveAwayFrom(block);
        }

        snowing.Update();
    }

    public override void Render(Batcher batcher)
    {
        Graphics.Clear(Color.Black);

        batcher.Image(nightSky, Vector2.Zero, Color.White);

        foreach (var block in blocks)
        {
            block.Draw(batcher);
        }

        player.Draw(batcher);

        snowing.Draw(batcher);
    }

    public override void Shutdown()
    {

    }
}
