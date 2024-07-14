﻿using Foster.Framework;
using Frosty.Scripts.Abstracts;
using Frosty.Scripts.Components;
using Frosty.Scripts.Core;
using Frosty.Scripts.DataStructures;
using Frosty.Scripts.Effects;
using Frosty.Scripts.Entities;
using Frosty.Scripts.Utils;
using System.Numerics;
using Timer = Frosty.Scripts.Components.Timer;

namespace Frosty.Scripts.Levels;

public class Level : Scene
{
    public bool Paused { get; private set; } = true;

    protected LevelEditor levelEditor;
    protected Player player;
    protected Snowing snowing;
    public float transitionOpacity;

    Timer playerDyingTimer;

    public override void Startup()
    {
        transitionOpacity = 1;
        levelEditor = new LevelEditor(true, new Tileset(["Assets", "Tilesets", "tileset.ase"], Game.TileSize, Game.TileSize, 8, 2, 12));
        snowing = new Snowing(new Vector2(0, 0), 0.005f, App.Width);

        player = new Player(new Vector2(100, 100));

        string filePath = Path.Combine("Assets", "Levels", $"{GetType().Name}.json");

        if (File.Exists(filePath))
        {
            levelEditor.Load(filePath);
        }

        playerDyingTimer = new Timer(1, true);
        
        playerDyingTimer.OnTimeout += () =>
        {
            Startup();
            player.Die = false;
        };
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

        playerDyingTimer.Update();

        if (Input.Keyboard.Pressed(Keys.Escape))
        {
            Game.SceneManager.ChangeScene("MainMenu");
        }

        player.Update();

        foreach (var tileObject in levelEditor.Tiles.Values)
        {
            switch (tileObject.tileType)
            {
                case TileType.Solid:
                    if (Helper.IsOverlapOnGround(player.Rect, tileObject.Rect))
                    {
                        player.friction = Game.entityFriction;
                        player.PlayWalkSound(tileObject.tileType);
                    }

                    player.ResolveAwayFrom(tileObject);
                    break;

                case TileType.Spike:
                    if (player.Rect.Overlaps(tileObject.Rect))
                    {
                        player.Die = true;
                    }
                    break;

                case TileType.Ice:
                    if (Helper.IsOverlapOnGround(player.Rect, tileObject.Rect))
                    {
                        player.friction = Game.entityOnIceFriction;
                        player.PlayWalkSound(tileObject.tileType);
                    }

                    player.ResolveAwayFrom(tileObject);
                    break;

                default:
                    break;
            }
        }

        if (player.position.Y > App.Height)
        {
            player.Die = true;
        }

        if (player.Die)
        {
            if (transitionOpacity < 0.9)
            {
                transitionOpacity += Time.Delta;
            }
            else
            {
                transitionOpacity = 1;
            }

            playerDyingTimer.Start();
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

        if (!player.Die)
        {
            if (transitionOpacity > 0.1)
            {
                transitionOpacity -= Time.Delta;
            }
            else
            {
                transitionOpacity = 0;
            }
        }

        batcher.Rect(0, 0, App.Width, App.Height, new Color(0, 0, 0, transitionOpacity));
    }

    public override void Shutdown()
    {

    }
}
