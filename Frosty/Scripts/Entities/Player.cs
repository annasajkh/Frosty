using Foster.Framework;
using Frosty.Scripts.Components;
using Frosty.Scripts.Core;
using System.Numerics;

namespace Frosty.Scripts.Entities;

public class Player : Entity
{
    float speed = 500;
    float jumpHeight = 500;

    public AnimationManager AnimationManager { get; } = new();

    public Player(Vector2 position) : base(position, 0, Vector2.One * 3, new Vector2(Application.playerIdle.Width, Application.playerIdle.Height))
    {
        AnimationManager.AddAnimation("player_idle", new Animation(Application.playerIdle, Application.playerIdle.Width, Application.playerIdle.Height, 1, true));
        AnimationManager.AddAnimation("player_walk_right", new Animation(Application.playerWalkRight, Application.playerWalkRight.Width, Application.playerWalkRight.Height, 0.25f, true));
        AnimationManager.AddAnimation("player_walk_left", new Animation(Application.playerWalkLeft, Application.playerWalkLeft.Width, Application.playerWalkLeft.Height, 0.25f, true));
    }

    public override void Update()
    {
        if (Input.Keyboard.Down(Keys.D))
        {
            if (IsOnGround)
            {
                AnimationManager.SetCurrent("player_walk_right");
                AnimationManager.Play();
            }
            else
            {
                AnimationManager.SetCurrent("player_walk_right");

                if (AnimationManager.CurrentAnimation is not null)
                {
                    AnimationManager.CurrentAnimation.FrameIndex = AnimationManager.CurrentAnimation.TotalFrames - 1;
                }

                AnimationManager.Stop();
            }

            velocity.X = speed;
        }
        else if (Input.Keyboard.Down(Keys.A))
        {
            if (IsOnGround)
            {
                AnimationManager.SetCurrent("player_walk_left");
                AnimationManager.Play();
            }
            else
            {
                AnimationManager.SetCurrent("player_walk_left");

                if (AnimationManager.CurrentAnimation is not null)
                {
                    AnimationManager.CurrentAnimation.FrameIndex = AnimationManager.CurrentAnimation.TotalFrames - 1;
                }

                AnimationManager.Stop();
            }

            velocity.X = -speed;
        }
        else
        {
            AnimationManager.SetCurrent("player_idle");
            AnimationManager.Play();
            velocity.X = 0;
        }

        if (Input.Keyboard.Down(Keys.Space) && IsOnGround)
        {
            velocity.Y = -jumpHeight;
        }

        base.Update();

        AnimationManager.Update();
        
    } 

    public void Draw(Batcher batcher)
    {
        batcher.PushMatrix(position, scale, size / 2, rotation);

        if (AnimationManager.CurrentAnimation is not null)
        {
            batcher.Image(AnimationManager.CurrentAnimation.CurrentFrame, Color.White);
        }

        batcher.PopMatrix();
    }
}
