using Frosty.Scripts.Components;
using Frosty.Scripts.Utils;
using System.Numerics;
using Foster.Framework;
using Timer = Frosty.Scripts.Components.Timer;
using Frosty.Scripts.GameObjects.Entities;
using Frosty.Scripts.GameObjects;
using Frosty.Scripts.GameObjects.Effects;
using Frosty.Scripts.Core;
using Frosty.Scripts.GameObjects.StaticTiles;
using Newtonsoft.Json;

namespace Frosty.Scripts.Scenes.Levels;

public abstract class Level : Scene
{
    protected enum NextLevelType
    {
        Right,
        Top,
        Bottom
    }

    public bool Paused { get; private set; }

    protected LevelEditor levelEditor;

    protected Player player;
    protected SnowSpawner snowSpawner;
    protected float transitionSpeed = 1;
    protected bool fadeOut;
    protected bool stopGameRuntime;

    protected event Action? fadeInTransitionFinished;
    protected event Action? fadeOutTransitionFinished;
    protected event Action? playerAtFinishLine;

    bool fadeInTransitionFinishedOnce;
    bool fadeOutTransitionFinishedOnce;
    bool playerAtFinishLineOnce;
    bool changeSceneRunOnce;

    public float transitionOpacity = 1;

    Timer playerDyingTimer;
    Timer playerWalkSoundEnableTimer;

    string filePath;

    protected DialogBox dialogBox;
    protected NextLevelType nextLevelType;

    List<BrittleIceBreakSpawner> brittleIceBreakSpawners = new();

    Timer saveTimer;

    public override void Startup()
    {
        Game.PlayerLevel = GetType().Name;

        string jsonSaveData = JsonConvert.SerializeObject(new SaveData(Game.PlayerDeath, Game.GameRunTime, Game.PlayerLevel), Formatting.Indented);
        File.WriteAllText("Save.json", jsonSaveData);

        saveTimer = new Timer(3, false);
        
        saveTimer.OnTimeout += () =>
        {
            string jsonSaveData = JsonConvert.SerializeObject(new SaveData(Game.PlayerDeath, Game.GameRunTime, Game.PlayerLevel), Formatting.Indented);
            File.WriteAllText("Save.json", jsonSaveData);
        };

        saveTimer.Start();


        nextLevelType = NextLevelType.Right;
        dialogBox = new DialogBox(new Vector2(App.Width / 2, 21 * Game.Scale + 5), 10);
        filePath = Path.Combine("Assets", "Levels", $"{GetType().Name}.json");

        levelEditor = new LevelEditor(true, EditingMode.TileSet, new TileMap(["Assets", "Graphics", "Tilesets", "tileset.ase"], Game.TileSize, Game.TileSize, 8, 8), new TileCollection(["Assets", "Graphics", "Tilesets", "decoration.ase"], [new Tile(Vector2.Zero, new Rect(0, 0, 36, 64), TileType.Decoration), new Tile(new Vector2(48, 0), new Rect(48, 0, 16, 16), TileType.Decoration)]), this);

#if DEBUG
        levelEditor.Editing = true;
#else
        levelEditor.Editing = false;
#endif

        snowSpawner = new SnowSpawner(new Vector2(0, 0), 0.005f, App.Width);
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
            Game.PlayerDeath++;
            levelEditor.Dispose();
            player.Dispose();
            dialogBox.Dispose();
            GC.Collect();

            Startup();
            player.Die = false;
        };
    }

    public void GoToNextLevel(string name)
    {
        fadeOut = true;
        player.noUpdate = true;

        fadeOutTransitionFinished += () =>
        {
            if (!changeSceneRunOnce)
            {
                Game.SceneManager.ChangeScene(name);
                changeSceneRunOnce = true;
            }
        };
    }


#if DEBUG
    public void SaveLevel()
    {
        levelEditor.Save(Path.Combine("Assets", "Levels", $"{GetType().Name}.json"));
        levelEditor.Save(Path.Combine("..", "..", "..", "Assets", "Levels", $"{GetType().Name}.json"));
    }
#endif

    public override void Update()
    {
        saveTimer.Update();

#if DEBUG
        if (Input.Keyboard.Down(Keys.LeftControl) && Input.Keyboard.Pressed(Keys.S))
        {
            SaveLevel();
        }
#endif

        if (!stopGameRuntime)
        {
            Game.GameRunTime += Time.Delta;
        }

        if (Input.Keyboard.Pressed(Keys.P))
        {
            Paused = !Paused;
        }

        levelEditor.Update();

        if (!Paused)
        {
            if (fadeOut)
            {
                if (transitionOpacity < 0.9999)
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

            playerWalkSoundEnableTimer.Update();
            dialogBox.Update();
            playerDyingTimer.Update();

            if (Input.Keyboard.Pressed(Keys.Escape))
            {
                Game.SceneManager.ChangeScene("MainMenu");
            }

            player.Update();

            if (player.Die)
            {
                if (transitionOpacity < 0.9999)
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

            foreach (var tileObject in levelEditor.Tiles.Reverse())
            {
                tileObject.Value.Update();

                if (Helper.IsOverlapOnGround(player.BoundingBox, tileObject.Value.BoundingBox))
                {
                    if (!player.playSoundWalkOnce)
                    {
                        if (!player.muteFootStep)
                        {
                            player.PlayWalkSound(tileObject.Value);
                        }
                        player.playSoundWalkOnce = true;
                    }

                    if (player.shouldPlayWalkSound && !player.Die)
                    {
                        if (!player.muteFootStep)
                        {
                            player.PlayWalkSound(tileObject.Value);
                        }

                        player.shouldPlayWalkSound = false;
                    }
                }
            }

            foreach (var tileObject in levelEditor.Tiles.Reverse())
            {
                tileObject.Value.ResolveCollision(player);

                if (tileObject.Value is BrittleIce brittleIce && brittleIce.Break)
                {
                    levelEditor.Tiles.Remove(tileObject.Key);
                    brittleIceBreakSpawners.Add(new BrittleIceBreakSpawner(brittleIce.position / 2, new Vector2(48, 48)));
                }
            }

            foreach (var brittleIceBreakSpawner in brittleIceBreakSpawners)
            {
                brittleIceBreakSpawner.Update();
            }

            for (int i = brittleIceBreakSpawners.Count - 1; i >= 0; i--)
            {
                if (brittleIceBreakSpawners[i].Deleted)
                {
                    brittleIceBreakSpawners.RemoveAt(i);
                }
            }

            switch (nextLevelType)
            {
                case NextLevelType.Right:
                    if (player.position.X > 1000 && !playerAtFinishLineOnce)
                    {
                        playerAtFinishLine?.Invoke();
                        playerAtFinishLineOnce = true;
                    }

                    break;
                case NextLevelType.Top:
                    if (player.position.Y < -player.size.Y * Game.Scale * 0.5 && !playerAtFinishLineOnce)
                    {
                        playerAtFinishLine?.Invoke();
                        playerAtFinishLineOnce = true;
                    }
                    break;
                case NextLevelType.Bottom:
                    if (player.position.Y > App.Height + player.size.Y * Game.Scale / 2 && player.position.X > App.Width / 2 && !playerAtFinishLineOnce)
                    {
                        playerAtFinishLine?.Invoke();
                        playerAtFinishLineOnce = true;
                    }
                    break;
            }

            if (player.position.X < player.size.X * Game.Scale / 2)
            {
                player.position.X = player.size.X * Game.Scale / 2;
            }

            if (player.position.Y > App.Height && nextLevelType != NextLevelType.Bottom)
            {
                player.Die = true;
            }

            snowSpawner.Update();
        }
    }

    public virtual void DrawHouse(Batcher batcher)
    {

    }


    public override void Render(Batcher batcher)
    {
        Graphics.Clear(Color.Black);

        batcher.Image(Game.NightSky, Vector2.Zero, Color.White);

        snowSpawner.Draw(batcher);

        DrawHouse(batcher);

        foreach (var tileObject in levelEditor.Tiles.Values)
        {
            if (tileObject is Decoration)
            {
                tileObject.Draw(batcher);
            }
        }

        foreach (var tileObject in levelEditor.Tiles.Values)
        {
            if (tileObject is not Decoration)
            {
                tileObject.Draw(batcher);
            }
        }

        player.Draw(batcher);
        levelEditor.Draw(batcher);

        foreach (var brittleIceBreakSpawner in brittleIceBreakSpawners)
        {
            brittleIceBreakSpawner.Draw(batcher);
        }

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
        levelEditor.Dispose();
        player.Dispose();
        dialogBox.Dispose();
    }
}
