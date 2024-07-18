using Foster.Framework;
using Frosty.Scripts.Components;
using Frosty.Scripts.Components.Audio;
using Frosty.Scripts.Core;
using Frosty.Scripts.GameObjects.Effects;
using Frosty.Scripts.GameObjects.StaticObjects;
using Frosty.Scripts.GameObjects.StaticTiles;
using System.Numerics;
using Timer = Frosty.Scripts.Components.Timer;
namespace Frosty.Scripts.GameObjects.Entities;

enum PreviousFacing
{
    Left,
    Right
}

public class Player : Entity
{
    public AnimationManager AnimationManager { get; } = new();
    public bool shouldPlayWalkSound;
    public bool playSoundWalkOnce;
    public bool freeze;
    public bool muteFootStep;

    float maxSpeed = 300;
    float jumpHeight = 500;
    bool spawnDieParticle;

    List<PlayerDieParticle> playerDieParticles = new();
    PreviousFacing previousFacing;

    SoundEffect[] playerWalkOnSnowSounds;
    SoundEffect[] playerWalkOnIceSounds;
    SoundEffect playerJump;
    SoundEffect playerDied;

    private static Aseprite playerIdleLeft = new Aseprite(Path.Combine("Assets", "Graphics", "Player", "player_idle_left.ase"));
    private static Aseprite playerIdleRight = new Aseprite(Path.Combine("Assets", "Graphics", "Player", "player_idle_right.ase"));

    private static Aseprite playerWalkRight = playerWalkRight = new Aseprite(Path.Combine("Assets", "Graphics", "Player", "player_walk_right.ase"));
    private static Aseprite playerWalkLeft = playerWalkLeft = new Aseprite(Path.Combine("Assets", "Graphics", "Player", "player_walk_left.ase"));

    Timer walkTimer;

    public override Rect BoundingBox
    {
        get
        {
            return new Rect(position - scale * size / 2 + new Vector2(10f, 0), position + scale * size / 2 - new Vector2(10f, 0));
        }
    }

    public Player(Vector2 position) : base(position, 0, Vector2.One * Game.Scale, new Vector2(playerIdleLeft.Width, playerIdleLeft.Height))
    {
        walkTimer = new Timer(0.25f, true);
        walkTimer.OnTimeout += () =>
        {
            shouldPlayWalkSound = true;
        };

        playerWalkOnSnowSounds = [SoundEffect.Load(Path.Combine("Assets", "Audio", "Sound Effects", "Snow Steps", "snow_step_0.ogg"), volume: 50),
                                  SoundEffect.Load(Path.Combine("Assets", "Audio", "Sound Effects", "Snow Steps", "snow_step_1.ogg"), volume: 50),
                                  SoundEffect.Load(Path.Combine("Assets", "Audio", "Sound Effects", "Snow Steps", "snow_step_2.ogg"), volume: 50)];

        playerWalkOnIceSounds = [SoundEffect.Load(Path.Combine("Assets", "Audio", "Sound Effects", "Ice Steps", "ice_step_0.ogg"), volume: 100),
                                 SoundEffect.Load(Path.Combine("Assets", "Audio", "Sound Effects", "Ice Steps", "ice_step_1.ogg"), volume: 100),
                                 SoundEffect.Load(Path.Combine("Assets", "Audio", "Sound Effects", "Ice Steps", "ice_step_2.ogg"), volume: 100)];

        playerJump = SoundEffect.Load(Path.Combine("Assets", "Audio", "Sound Effects", "Player", "player_jump.ogg"), volume: 100);
        playerDied = SoundEffect.Load(Path.Combine("Assets", "Audio", "Sound Effects", "Player", "player_died.ogg"), volume: 255);

        AnimationManager.AddAnimation("player_idle_left", new Animation(playerIdleLeft, playerIdleLeft.Width, playerIdleLeft.Height, 0.5f, true));
        AnimationManager.AddAnimation("player_idle_right", new Animation(playerIdleRight, playerIdleRight.Width, playerIdleRight.Height, 0.5f, true));

        AnimationManager.AddAnimation("player_walk_right", new Animation(playerWalkRight, playerWalkRight.Width, playerWalkRight.Height, 0.25f, true));
        AnimationManager.AddAnimation("player_walk_left", new Animation(playerWalkLeft, playerWalkLeft.Width, playerWalkLeft.Height, 0.25f, true));

        previousFacing = PreviousFacing.Right;
    }

    public void PlayWalkSound(TileObject tileObject)
    {
        switch (tileObject)
        {
            case Solid:
                Game.SoundEffectPlayer.SetSource(playerWalkOnSnowSounds[Game.Random.Next() % playerWalkOnSnowSounds.Length]);
                Game.SoundEffectPlayer.Play();
                break;
            case Ice:
                Game.SoundEffectPlayer.SetSource(playerWalkOnIceSounds[Game.Random.Next() % playerWalkOnIceSounds.Length]);
                Game.SoundEffectPlayer.Play();
                break;
            default:
                break;
        }
    }

    public override void Update()
    {
        foreach (var playerDieParticle in playerDieParticles)
        {
            playerDieParticle.Update();
        }

        for (int i = playerDieParticles.Count - 1; i >= 0; i--)
        {
            if (playerDieParticles[i].Destroyed)
            {
                playerDieParticles.RemoveAt(i);
            }
        }

        if (Die && !spawnDieParticle)
        {
            Game.SoundEffectPlayer.SetSource(playerDied);
            Game.SoundEffectPlayer.Play();

            for (int i = 0; i < 20; i++)
            {
                playerDieParticles.Add(new PlayerDieParticle(position, new Vector2(1, 1), 0.3f, 200));
            }
            spawnDieParticle = true;
        }

        if (!Die)
        {
            if (velocity.Y < 0)
            {
                playSoundWalkOnce = false;
            }

            walkTimer.Update();

            if (Input.Keyboard.Down(Keys.D) && !freeze)
            {
                if (MayJump > 0)
                {
                    AnimationManager.SetCurrent("player_walk_right");
                    AnimationManager.Play();
                }

                if (!IsOnGround)
                {
                    AnimationManager.SetCurrent("player_walk_right");

                    if (AnimationManager.CurrentAnimation is not null)
                    {
                        AnimationManager.CurrentAnimation.FrameIndex = AnimationManager.CurrentAnimation.TotalFrames - 1;
                    }

                    AnimationManager.Stop();
                }

                velocity.X += Speed;
                previousFacing = PreviousFacing.Right;

                if (walkTimer.Paused)
                {
                    walkTimer.Start();
                }
            }
            else if (Input.Keyboard.Down(Keys.A) && !freeze)
            {
                if (MayJump > 0)
                {
                    AnimationManager.SetCurrent("player_walk_left");
                    AnimationManager.Play();
                }

                if (!IsOnGround)
                {
                    AnimationManager.SetCurrent("player_walk_left");

                    if (AnimationManager.CurrentAnimation is not null)
                    {
                        AnimationManager.CurrentAnimation.FrameIndex = AnimationManager.CurrentAnimation.TotalFrames - 1;
                    }

                    AnimationManager.Stop();
                }

                velocity.X += -Speed;
                previousFacing = PreviousFacing.Left;

                if (walkTimer.Paused)
                {
                    walkTimer.Start();
                }
            }
            else
            {
                switch (previousFacing)
                {
                    case PreviousFacing.Left:
                        AnimationManager.SetCurrent("player_idle_left");
                        break;
                    case PreviousFacing.Right:
                        AnimationManager.SetCurrent("player_idle_right");
                        break;
                }

                AnimationManager.Play();

                walkTimer.Stop();
            }

            if (Input.Keyboard.Pressed(Keys.Space) && (IsOnGround || MayJump > 0) && !freeze)
            {
                Game.SoundEffectPlayer.SetSource(playerJump);
                Game.SoundEffectPlayer.Play();
                velocity.Y = -jumpHeight;
                MayJump = 0;
                IsOnGround = false;
            }

            velocity.X = Math.Clamp(velocity.X, -maxSpeed, maxSpeed);

            base.Update();
            AnimationManager.Update();
        }
    }

    public override void Draw(Batcher batcher)
    {
        if (!Die)
        {
            if (AnimationManager.CurrentAnimation is not null)
            {
                batcher.PushMatrix(position, scale, size / 2, rotation);
                batcher.Image(AnimationManager.CurrentAnimation.CurrentFrame, Color.White);
                batcher.PopMatrix();
            }
        }

        foreach (var playerDieParticle in playerDieParticles)
        {
            playerDieParticle.Draw(batcher);
        }

        base.Draw(batcher);
    }
}
