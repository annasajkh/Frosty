using Frosty.Scripts.Components;
using Frosty.Scripts.Utils;
using System.Numerics;
using Foster.Framework;
using Timer = Frosty.Scripts.Components.Timer;
using Frosty.Scripts.GameObjects.Entities;
using Frosty.Scripts.GameObjects;
using Frosty.Scripts.GameObjects.Effects;
using Frosty.Scripts.Core;

namespace Frosty.Scripts.Scenes.Levels;

public class Level : Scene
{
    public bool Paused { get; private set; } = false;

    protected LevelEditor levelEditor;
    
    protected Player player;
    protected Snowing snowing;
    protected float transitionSpeed = 1;
    protected bool fadeOut = false;

    protected event Action? fadeInTransitionFinished;
    protected event Action? fadeOutTransitionFinished;
    protected event Action? playerAtFinishLine;

    bool fadeInTransitionFinishedOnce = false;
    bool fadeOutTransitionFinishedOnce = false;
    bool playerAtFinishLineOnce = false;

    public float transitionOpacity;

    Timer playerDyingTimer;
    Timer autoSaveTimer;
    Timer playerWalkSoundEnableTimer;

    string filePath;

    protected DialogBox dialogBox;

    public override void Startup()
    {
        dialogBox = new DialogBox(new Vector2(App.Width / 2, 21 * Game.Scale + 5), 10);
        transitionOpacity = 1;
        filePath = Path.Combine("Assets", "Levels", $"{GetType().Name}.json");

        levelEditor = new LevelEditor(true, EditingMode.TileSet, new TileMap(["Assets", "Tilesets", "tileset.ase"], Game.TileSize, Game.TileSize, 8, 2), new TileCollection(["Assets", "Backgrounds", "decoration.ase"], [new Tile(Vector2.Zero, new Rect(0, 0, 36, 64), TileType.Decoration), new Tile(new Vector2(48, 0), new Rect(48, 0, 16, 16), TileType.Decoration)]), this);

#if DEBUG
        levelEditor.Editing = true;
#else
        levelEditor.Editing = false;
#endif

        snowing = new Snowing(new Vector2(0, 0), 0.005f, App.Width);
        player = new Player(new Vector2(100, 100));
        player.muteFootStep = true;

        playerWalkSoundEnableTimer = new Timer(0.15f, true);
        playerWalkSoundEnableTimer.OnTimeout += () =>
        {
            player.muteFootStep = false;
        };
        playerWalkSoundEnableTimer.Start();

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

        autoSaveTimer = new Timer(1, false);
        autoSaveTimer.OnTimeout += () =>
        {
            SaveLevel();
        };

        autoSaveTimer.Start();
    }

    public void GoToNextLevel(string name)
    {
        fadeOut = true;
        player.freeze = true;

        fadeOutTransitionFinished += () =>
        {
            Game.SceneManager.ChangeScene(name);
        };
    }

    public void SaveLevel()
    {
        levelEditor.Save(Path.Combine("Assets", "Levels", $"{GetType().Name}.json"));

#if DEBUG
        try
        {
            levelEditor.Save(Path.Combine("..", "..", "..", "Assets", "Levels", $"{GetType().Name}.json"));
        }
        finally
        {
            // Ignore the fucking error
        }
#endif
        if (File.Exists(filePath))
        {
            levelEditor.Tiles.Clear();
            levelEditor.Load(filePath);
        }
    }

    public override void Update()
    {
        autoSaveTimer.Update();

        if (Input.Keyboard.Down(Keys.LeftControl) && Input.Keyboard.Pressed(Keys.S))
        {
            SaveLevel();
        }

        if (Input.Keyboard.Pressed(Keys.P))
        {
            Paused = !Paused;
        }

        levelEditor.Update();

        if (!Paused)
        {
            playerWalkSoundEnableTimer.Update();

            if (player.position.X > 1000 && !playerAtFinishLineOnce)
            {
                playerAtFinishLine?.Invoke();
                playerAtFinishLineOnce = true;
            }

            dialogBox.Update();
            playerDyingTimer.Update();

            if (Input.Keyboard.Pressed(Keys.Escape))
            {
                Game.SceneManager.ChangeScene("MainMenu");
            }

            player.Update();

            foreach (var tileObject in levelEditor.Tiles.Values)
            {
                if (Helper.IsOverlapOnGround(player.BoundingBox, tileObject.BoundingBox))
                {
                    if (!player.playSoundWalkOnce)
                    {
                        if (!player.muteFootStep)
                        {
                            player.PlayWalkSound(tileObject.tileType);
                        }
                        player.playSoundWalkOnce = true;
                    }

                    if (player.shouldPlayWalkSound && !player.Die)
                    {
                        if (!player.muteFootStep)
                        {
                            player.PlayWalkSound(tileObject.tileType);
                        }

                        player.shouldPlayWalkSound = false;
                    }
                }

                switch (tileObject.tileType)
                {
                    case TileType.Solid:
                        if (Helper.IsOverlapOnGround(player.BoundingBox, tileObject.BoundingBox))
                        {
                            player.friction = Game.entityFriction;
                        }

                        player.ResolveAwayFrom(tileObject);
                        break;

                    case TileType.Spike:
                        if (player.BoundingBox.Overlaps(tileObject.BoundingBox))
                        {
                            player.Die = true;
                        }
                        break;

                    case TileType.Ice:
                        if (Helper.IsOverlapOnGround(player.BoundingBox, tileObject.BoundingBox))
                        {
                            player.friction = Game.entityOnIceFriction;
                        }

                        player.ResolveAwayFrom(tileObject);
                        break;

                    default:
                        break;
                }
            }

            if (player.position.X < player.size.X * Game.Scale / 2)
            {
                player.position.X = player.size.X * Game.Scale / 2;
            }

            if (player.position.Y < player.size.Y * Game.Scale / 2)
            {
                player.position.Y = player.size.Y * Game.Scale / 2;
                player.velocity.Y = 0;
            }

            if (player.position.Y > App.Height)
            {
                player.Die = true;
            }

            if (fadeOut)
            {
                if (transitionOpacity < 0.9)
                {
                    fadeOutTransitionFinishedOnce = false;
                    transitionOpacity += Time.Delta * transitionSpeed;
                }
                else
                {
                    if (!fadeOutTransitionFinishedOnce)
                    {
                        fadeOutTransitionFinished?.Invoke();
                        fadeOutTransitionFinishedOnce = true;
                    }
                    transitionOpacity = 1;
                }
            }

            if (player.Die)
            {
                if (transitionOpacity < 0.9)
                {
                    fadeOutTransitionFinishedOnce = false;
                    transitionOpacity += Time.Delta * transitionSpeed;
                }
                else
                {
                    if (!fadeOutTransitionFinishedOnce)
                    {
                        fadeOutTransitionFinished?.Invoke();
                        fadeOutTransitionFinishedOnce = true;
                    }
                    transitionOpacity = 1;
                }

                playerDyingTimer.Start();
            }

            snowing.Update();
        }
    }

    public override void Render(Batcher batcher)
    {
        Graphics.Clear(Color.Black);

        batcher.Image(Game.NightSky, Vector2.Zero, Color.White);

        snowing.Draw(batcher);

        foreach (var tileObject in levelEditor.Tiles.Values)
        {
            if (tileObject.tileType == TileType.Decoration)
            {
                tileObject.Draw(batcher);
            }
        }

        foreach (var tileObject in levelEditor.Tiles.Values)
        {
            if (tileObject.tileType != TileType.Decoration)
            {
                tileObject.Draw(batcher);
            }
        }

        player.Draw(batcher);

        levelEditor.Draw(batcher);

        if (Paused)
        {
            transitionOpacity = 0.75f;
        }

        if (!player.Die && !fadeOut)
        {
            if (transitionOpacity > 0.1)
            {
                fadeInTransitionFinishedOnce = false;
                transitionOpacity -= Time.Delta * transitionSpeed;
            }
            else
            {
                if (!fadeInTransitionFinishedOnce)
                {
                    fadeInTransitionFinished?.Invoke();
                    fadeInTransitionFinishedOnce = true;
                }
                transitionOpacity = 0;
            }
        }

        dialogBox.Draw(batcher);

        batcher.Rect(0, 0, App.Width, App.Height, new Color(0, 0, 0, transitionOpacity));

        if (Paused)
        {
            levelEditor.DrawWhenPaused(batcher);
            Helper.DrawTextCentered("Paused", new Vector2(App.Width / 2, App.Height / 2), Color.White, Game.M5x7Menu, batcher);
        }
    }

    public override void Shutdown()
    {

    }
}
