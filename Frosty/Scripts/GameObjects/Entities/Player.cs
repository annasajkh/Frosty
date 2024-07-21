using Foster.Framework;
using Frosty.Scripts.Components;
using Frosty.Scripts.Core;
using Frosty.Scripts.GameObjects.Effects;
using Frosty.Scripts.GameObjects.StaticObjects;
using Frosty.Scripts.GameObjects.StaticTiles;
using System.Numerics;
using Timer = Frosty.Scripts.Components.Timer;
namespace Frosty.Scripts.GameObjects.Entities;

enum Facing
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
    public bool noUpdate;
    public bool walkRight;

    float maxSpeed = 300;
    float jumpHeight = 500;
    bool spawnDieParticle;

    List<PlayerDieParticle> playerDieParticles = new();
    Facing previousFacing;

    Timer walkTimer;

    public override Rect BoundingBox
    {
        get
        {
            return new Rect(position - scale * size / 2 + new Vector2(10f, 10), position + scale * size / 2 - new Vector2(10f, 0));
        }
    }

    public Player(Vector2 position) : base(position, 0, Vector2.One * Game.Scale, new Vector2(Game.playerIdleLeft.Width, Game.playerIdleLeft.Height))
    {
        walkTimer = new Timer(0.25f, true);
        walkTimer.OnTimeout += () =>
        {
            shouldPlayWalkSound = true;
        };

        AnimationManager.AddAnimation("player_idle_left", new Animation(Game.playerIdleLeft, Game.playerIdleLeft.Width, Game.playerIdleLeft.Height, 0.5f, true));
        AnimationManager.AddAnimation("player_idle_right", new Animation(Game.playerIdleRight, Game.playerIdleRight.Width, Game.playerIdleRight.Height, 0.5f, true));

        AnimationManager.AddAnimation("player_walk_right", new Animation(Game.playerWalkRight, Game.playerWalkRight.Width, Game.playerWalkRight.Height, 0.25f, true));
        AnimationManager.AddAnimation("player_walk_left", new Animation(Game.playerWalkLeft, Game.playerWalkLeft.Width, Game.playerWalkLeft.Height, 0.25f, true));

        previousFacing = Facing.Right;
    }

    public void PlayWalkSound(TileObject tileObject)
    {
        switch (tileObject)
        {
            case Solid:
                Game.playerWalkOnSnowSounds[Game.Random.Next() % Game.playerWalkOnSnowSounds.Length].Play();
                break;
            case Ice:
                Game.playerWalkOnIceSounds[Game.Random.Next() % Game.playerWalkOnIceSounds.Length].Play();
                break;
            case BrittleIce:
                Game.playerWalkOnIceSounds[Game.Random.Next() % Game.playerWalkOnIceSounds.Length].Play();
                break;
            default:
                break;
        }
    }

    public override void Update()
    {
        if (noUpdate)
        {
            return;
        }

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
            Game.playerDied.Play();

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

            if ((Input.Keyboard.Down(Keys.D) && !freeze) || walkRight)
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

                velocity.X += speed;
                previousFacing = Facing.Right;

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

                velocity.X += -speed;
                previousFacing = Facing.Left;

                if (walkTimer.Paused)
                {
                    walkTimer.Start();
                }
            }
            else
            {
                switch (previousFacing)
                {
                    case Facing.Left:
                        AnimationManager.SetCurrent("player_idle_left");
                        break;
                    case Facing.Right:
                        AnimationManager.SetCurrent("player_idle_right");
                        break;
                }

                AnimationManager.Play();

                walkTimer.Stop();
            }

            if (Input.Keyboard.Pressed(Keys.Space) && (IsOnGround || MayJump > 0) && !freeze)
            {
                Game.playerJump.Play();
                velocity.Y += -jumpHeight;
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

    public override void Dispose()
    {
        base.Dispose();
    }
}
