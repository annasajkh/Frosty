using Foster.Framework;
using Frosty.Scripts.Audio;
using Frosty.Scripts.Levels;
using Frosty.Scripts.Managers;
using Frosty.Scripts.Scenes;

namespace Frosty.Scripts.Core;

public sealed class Game : Module
{
    public static float entityFriction = 0.3f;
    public static float entityOnIceFriction = 0.05f;
    public static float Scale { get; } = 3;
    public static int TileSize { get; } = 16;

    public static bool DebugMode { get; } = false;

    public static SpriteFont M5x7Menu { get; } = new SpriteFont(Path.Combine("Assets", "Fonts", "m5x7.ttf"), 64);
    public static SpriteFont M5x7Dialog { get; } = new SpriteFont(Path.Combine("Assets", "Fonts", "m5x7.ttf"), 32);

    public static Texture NightSky { get; } = new Texture(new Aseprite(Path.Combine("Assets", "Backgrounds", "night_sky.ase")).Frames[0].Cels[0].Image);

    public static float gravity = 20;
    public static Random Random { get; } = new(Time.Now.Milliseconds);
    public static SceneManager SceneManager { get; private set; } = new(initialSceneName: "MainMenu", initialScene: new MainMenu());
    public static SoundEffectPlayer SoundEffectPlayer { get; private set; } = new SoundEffectPlayer();
    public static SoundEffect PlayerTalk { get; } = SoundEffect.Load(Path.Combine("Assets", "Player", "Audio", "Sound Effects", "player_talk.ogg"), volume: 20);

    Batcher batcher = new();

    public override void Startup()
    {
        SceneManager.AddScene("IntroLevel", new IntroLevel());
        SceneManager.AddScene("Level1", new Level1());
    }

    public override void Update()
    {
        SceneManager.ActiveScene?.Update();
    }

    public override void Render()
    {
        SceneManager.ActiveScene?.Render(batcher);

        batcher.Render();
        batcher.Clear();
    }

    public override void Shutdown()
    {
        SceneManager.ActiveScene?.Shutdown();
    }
}
