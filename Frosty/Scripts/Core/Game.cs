using Foster.Framework;
using Frosty.Scripts.Components;
using Frosty.Scripts.Scenes;
using Frosty.Scripts.Scenes.Levels;
using SFML.Audio;

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
    public static Sound PlayerTalk { get; } = new Sound(new SoundBuffer(Path.Combine("Assets", "Audio", "Sound Effects", "Player", "player_talk.ogg")));

    public static Texture dialogBoxTexture = new Texture(new Aseprite(Path.Combine("Assets", "Graphics", "UIs", "dialog_box.ase")).Frames[0].Cels[0].Image);

    public static Texture houseTexture = new Texture(new Aseprite(Path.Combine("Assets", "Graphics", "Buildings", "house.ase")).Frames[0].Cels[0].Image);

    public static Sound[] playerWalkOnSnowSounds;
    public static Sound[] playerWalkOnIceSounds;
    public static Sound playerJump;
    public static Sound playerDied;
    public static Sound[] iceBreakingSounds;

    public static Aseprite playerIdleLeft = new Aseprite(Path.Combine("Assets", "Graphics", "Player", "player_idle_left.ase"));
    public static Aseprite playerIdleRight = new Aseprite(Path.Combine("Assets", "Graphics", "Player", "player_idle_right.ase"));

    public static Aseprite playerWalkRight = playerWalkRight = new Aseprite(Path.Combine("Assets", "Graphics", "Player", "player_walk_right.ase"));
    public static Aseprite playerWalkLeft = playerWalkLeft = new Aseprite(Path.Combine("Assets", "Graphics", "Player", "player_walk_left.ase"));

    Batcher batcher = new();

    public override void Startup()
    {
        PlayerTalk.Volume = 20;

        playerWalkOnSnowSounds = [new Sound(new SoundBuffer(Path.Combine("Assets", "Audio", "Sound Effects", "Snow Steps", "snow_step_0.ogg"))),
                                  new Sound(new SoundBuffer(Path.Combine("Assets", "Audio", "Sound Effects", "Snow Steps", "snow_step_1.ogg"))),
                                  new Sound(new SoundBuffer(Path.Combine("Assets", "Audio", "Sound Effects", "Snow Steps", "snow_step_2.ogg")))];

        playerWalkOnIceSounds = [new Sound(new SoundBuffer(Path.Combine("Assets", "Audio", "Sound Effects", "Ice Steps", "ice_step_0.ogg"))),
                                 new Sound(new SoundBuffer(Path.Combine("Assets", "Audio", "Sound Effects", "Ice Steps", "ice_step_1.ogg"))),
                                 new Sound(new SoundBuffer(Path.Combine("Assets", "Audio", "Sound Effects", "Ice Steps", "ice_step_2.ogg")))];

        playerJump = new Sound(new SoundBuffer(Path.Combine("Assets", "Audio", "Sound Effects", "Player", "player_jump.ogg")));
        playerJump.Volume = 50;
        playerDied = new Sound(new SoundBuffer(Path.Combine("Assets", "Audio", "Sound Effects", "Player", "player_died.ogg")));

        iceBreakingSounds = [new Sound(new SoundBuffer(Path.Combine("Assets", "Audio", "Sound Effects", "Ice Breaking", "ice_breaking_0.ogg"))),
                             new Sound(new SoundBuffer(Path.Combine("Assets", "Audio", "Sound Effects", "Ice Breaking", "ice_breaking_1.ogg"))),
                             new Sound(new SoundBuffer(Path.Combine("Assets", "Audio", "Sound Effects", "Ice Breaking", "ice_breaking_2.ogg")))];

        foreach (var breakingSound in iceBreakingSounds)
        {
            breakingSound.Volume = 50;
        }

        foreach (var playerWalkOnSnowSound in playerWalkOnSnowSounds)
        {
            playerWalkOnSnowSound.Volume = 50;
        }

        foreach (var playerWalkOnIceSound in playerWalkOnIceSounds)
        {
            playerWalkOnIceSound.Volume = 100;
        }

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
