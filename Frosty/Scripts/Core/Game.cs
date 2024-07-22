using Foster.Framework;
using Frosty.Scripts.Components;
using Frosty.Scripts.Components.Audio;
using Frosty.Scripts.Scenes;
using Frosty.Scripts.Scenes.Levels;

namespace Frosty.Scripts.Core;

public sealed class Game : Module
{
    public static string? PlayerLevel { get; set; }
    public static int PlayerDeath { get; set; }
    public static double GameRunTime { get; set; }

    public static float entityFriction = 0.3f;
    public static float entityOnIceFriction = 0.1f;
    public static float Scale { get; } = 3;
    public static int TileSize { get; } = 16;

    public static bool ShowColliders { get; } = false;

    public static SpriteFont M5x7Menu { get; } = new SpriteFont(Path.Combine("Assets", "Fonts", "m5x7.ttf"), 64);
    public static SpriteFont M5x7Dialog { get; } = new SpriteFont(Path.Combine("Assets", "Fonts", "m5x7.ttf"), 32);

    public static Texture NightSky { get; } = new Texture(new Aseprite(Path.Combine("Assets", "Graphics", "Backgrounds", "night_sky.ase")).Frames[0].Cels[0].Image);

    public static float gravity = 20;
    public static Random Random { get; } = new(Time.Now.Milliseconds);
    public static SceneManager SceneManager { get; private set; } = new(initialSceneName: "MainMenu", initialScene: new MainMenu());
    public static SoundEffect PlayerTalk { get; } = SoundEffect.Load(Path.Combine("Assets", "Audio", "Sound Effects", "Player", "player_talk.ogg"));

    public static Texture dialogBoxTexture = new Texture(new Aseprite(Path.Combine("Assets", "Graphics", "UIs", "dialog_box.ase")).Frames[0].Cels[0].Image);

    public static Texture houseTexture = new Texture(new Aseprite(Path.Combine("Assets", "Graphics", "Buildings", "house.ase")).Frames[0].Cels[0].Image);

    public static SoundEffect[] playerWalkOnSnowSounds;
    public static SoundEffect[] playerWalkOnIceSounds;
    public static SoundEffect playerJump;
    public static SoundEffect playerDied;
    public static SoundEffect[] iceBreakingSounds;

    public static SoundEffectPlayer SoundEffectPlayer { get; private set; } = new SoundEffectPlayer();

    public static Aseprite playerIdleLeft = new Aseprite(Path.Combine("Assets", "Graphics", "Player", "player_idle_left.ase"));
    public static Aseprite playerIdleRight = new Aseprite(Path.Combine("Assets", "Graphics", "Player", "player_idle_right.ase"));

    public static Aseprite playerWalkRight = playerWalkRight = new Aseprite(Path.Combine("Assets", "Graphics", "Player", "player_walk_right.ase"));
    public static Aseprite playerWalkLeft = playerWalkLeft = new Aseprite(Path.Combine("Assets", "Graphics", "Player", "player_walk_left.ase"));

    Batcher batcher = new();

    public override void Startup()
    {
        PlayerTalk.Volume = 20;

        playerWalkOnSnowSounds = [SoundEffect.Load(Path.Combine("Assets", "Audio", "Sound Effects", "Snow Steps", "snow_step_0.ogg"), volume: 50),
                                  SoundEffect.Load(Path.Combine("Assets", "Audio", "Sound Effects", "Snow Steps", "snow_step_1.ogg"), volume: 50),
                                  SoundEffect.Load(Path.Combine("Assets", "Audio", "Sound Effects", "Snow Steps", "snow_step_2.ogg"), volume: 50)];

        playerWalkOnIceSounds = [SoundEffect.Load(Path.Combine("Assets", "Audio", "Sound Effects", "Ice Steps", "ice_step_0.ogg"), volume: 100),
                                 SoundEffect.Load(Path.Combine("Assets", "Audio", "Sound Effects", "Ice Steps", "ice_step_1.ogg"), volume: 100),
                                 SoundEffect.Load(Path.Combine("Assets", "Audio", "Sound Effects", "Ice Steps", "ice_step_2.ogg"), volume: 100)];

        playerJump = SoundEffect.Load(Path.Combine("Assets", "Audio", "Sound Effects", "Player", "player_jump.ogg"), volume: 100);
        playerDied = SoundEffect.Load(Path.Combine("Assets", "Audio", "Sound Effects", "Player", "player_died.ogg"), volume: 100);


        iceBreakingSounds = [SoundEffect.Load(Path.Combine("Assets", "Audio", "Sound Effects", "Ice Breaking", "ice_breaking_0.ogg"), volume: 50),
                             SoundEffect.Load(Path.Combine("Assets", "Audio", "Sound Effects", "Ice Breaking", "ice_breaking_1.ogg"), volume: 50),
                             SoundEffect.Load(Path.Combine("Assets", "Audio", "Sound Effects", "Ice Breaking", "ice_breaking_2.ogg"), volume: 50)];

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
        SceneManager.AddScene("LevelL", new LevelL());
        SceneManager.AddScene("LevelM", new LevelM());
        SceneManager.AddScene("LevelN", new LevelN());
        SceneManager.AddScene("LevelO", new LevelO());
        SceneManager.AddScene("LevelP", new LevelP());
        SceneManager.AddScene("LevelQ", new LevelQ());
        SceneManager.AddScene("LevelR", new LevelR());

#if DEBUG
        //SceneManager.ChangeScene("IntroLevel");
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

        NightSky.Dispose();
        PlayerTalk.Dispose();
    }
}
