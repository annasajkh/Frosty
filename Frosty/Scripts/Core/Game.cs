using Foster.Framework;
using Frosty.Scripts.Components;
using Frosty.Scripts.Components.Audio;
using Frosty.Scripts.Scenes;
using Frosty.Scripts.Scenes.Levels;

namespace Frosty.Scripts.Core;

public sealed class Game : Module
{
    public static float entityFriction = 0.3f;
    public static float entityOnIceFriction = 0.05f;
    public static float Scale { get; } = 3;
    public static int TileSize { get; } = 16;

    public static bool ShowColliders { get; } = false;

    public static SpriteFont M5x7Menu { get; } = new SpriteFont(Path.Combine("Assets", "Fonts", "m5x7.ttf"), 64);
    public static SpriteFont M5x7Dialog { get; } = new SpriteFont(Path.Combine("Assets", "Fonts", "m5x7.ttf"), 32);

    public static Texture NightSky { get; } = new Texture(new Aseprite(Path.Combine("Assets", "Graphics", "Backgrounds", "night_sky.ase")).Frames[0].Cels[0].Image);

    public static float gravity = 20;
    public static Random Random { get; } = new(Time.Now.Milliseconds);
    public static SceneManager SceneManager { get; private set; } = new(initialSceneName: "MainMenu", initialScene: new MainMenu());
    public static SoundEffectPlayer SoundEffectPlayer { get; private set; } = new SoundEffectPlayer();
    public static SoundEffect PlayerTalk { get; } = SoundEffect.Load(Path.Combine("Assets", "Audio", "Sound Effects", "Player", "player_talk.ogg"), volume: 20);

    Batcher batcher = new();

    public override void Startup()
    {
        SceneManager.AddScene("IntroLevel", new IntroLevel());
        SceneManager.AddScene("LevelA", new LevelA());
        SceneManager.AddScene("LevelB", new LevelB());
        SceneManager.AddScene("LevelC", new LevelC());
        SceneManager.AddScene("LevelD", new LevelD());
        SceneManager.AddScene("LevelE", new LevelE());
        SceneManager.AddScene("LevelF", new LevelF());
        SceneManager.AddScene("LevelG", new LevelG());
        SceneManager.AddScene("LevelH", new LevelH());
        SceneManager.AddScene("LevelI", new LevelI());
        SceneManager.AddScene("LevelJ", new LevelJ());
        SceneManager.AddScene("LevelK", new LevelK());

#if DEBUG
        SceneManager.ChangeScene("LevelB");
#endif
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
