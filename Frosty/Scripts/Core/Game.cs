using Foster.Framework;
using Frosty.Scripts.Audio;
using Frosty.Scripts.Levels;
using Frosty.Scripts.Managers;
using Frosty.Scripts.Scenes;

namespace Frosty.Scripts.Core;

public sealed class Game : Module
{
    public static float entityFriction = 0.3f;
    public static float entityOnIceFriction = 0.01f;
    public static float Scale { get; } = 3;
    public static int TileSize { get; } = 16;

    public static bool DebugMode { get; } = false;

    public static SpriteFont ArialFont { get; } = new SpriteFont(Path.Combine("Assets", "Fonts", "Arial.ttf"), 32);
    public static Texture NightSky { get; } = new Texture(new Aseprite(Path.Combine("Assets", "Backgrounds", "night_sky.ase")).Frames[0].Cels[0].Image);

    public static float gravity = 20;
    public static Random Random { get; } = new(Time.Now.Milliseconds);
    public static SceneManager SceneManager { get; private set; } = new(initialSceneName: "MainMenu", initialScene: new MainMenu());
    public static SoundEffectPlayer SoundEffectPlayer { get; private set; } = new SoundEffectPlayer();

    Batcher batcher = new();

    public override void Startup()
    {
        SceneManager.AddScene("TestLevel", new TestLevel());
        SceneManager.ChangeScene("TestLevel");
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
