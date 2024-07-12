using Foster.Framework;
using Frosty.Scripts.Abstracts;
using Frosty.Scripts.Components;
using Frosty.Scripts.Core;
using Frosty.Scripts.Entities;
using Frosty.Scripts.Utils;
using System.Numerics;

namespace Frosty.Scripts.Levels;

public class Level : Scene
{
    public bool Paused { get; private set; } = true;

    protected LevelEditor levelEditor;
    protected Player player;
    protected Snowing snowing;

    public override void Startup()
    {
        levelEditor = new LevelEditor(true, new Tileset(["Assets", "Tilesets", "ground.ase"], Game.TileSize, Game.TileSize, 8, 2, 9));
        snowing = new Snowing(new Vector2(0, 0), 0.005f, App.Width);

        player = new Player(new Vector2(100, 100));

        levelEditor.Load(Path.Combine("Assets", "Levels", $"{GetType().Name}.json"));
    }

    public override void Update()
    {
        if (Input.Keyboard.Down(Keys.LeftControl) && Input.Keyboard.Down(Keys.S))
        {
            levelEditor.Save(Path.Combine("..", "..", "..", "Assets", "Levels", $"{GetType().Name}.json"));
        }

        if (Input.Keyboard.Pressed(Keys.P))
        {
            Paused = !Paused;
        }

        levelEditor.Update();

        if (Paused)
        {
            return;
        }

        if (Input.Keyboard.Pressed(Keys.Escape))
        {
            Game.SceneManager.ChangeScene("MainMenu");
        }

        player.Update();

        foreach (var tileObject in levelEditor.Tiles.Values)
        {
            player.ResolveAwayFrom(tileObject);
        }

        snowing.Update();
    }

    public override void Render(Batcher batcher)
    {
        Graphics.Clear(Color.Black);

        batcher.Image(Game.NightSky, Vector2.Zero, Color.White);

        snowing.Draw(batcher);

        foreach (var tileObject in levelEditor.Tiles.Values)
        {
            tileObject.Draw(batcher);
        }

        player.Draw(batcher);

        levelEditor.Draw(batcher);

        if (Paused)
        {
            levelEditor.DrawWhenPaused(batcher);
            Helper.DrawTextCentered("Paused", new Vector2(App.Width / 2, App.Height / 2), Color.White, Game.ArialFont, batcher);
        }
    }

    public override void Shutdown()
    {

    }
}
