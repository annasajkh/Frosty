﻿using Foster.Framework;
using Frosty.Scripts.Components;
using Frosty.Scripts.Core;
using System.Numerics;

namespace Frosty.Scripts.Entities;


public class Player : Entity
{
    enum PreviousFacing
    {
        Left,
        Right
    }

    private static Aseprite playerIdleLeft = new Aseprite(Path.Combine("Assets", "Player", "player_idle_left.ase"));
    private static Aseprite playerIdleRight = new Aseprite(Path.Combine("Assets", "Player", "player_idle_right.ase"));

    private static Aseprite playerWalkRight = playerWalkRight = new Aseprite(Path.Combine("Assets", "Player", "player_walk_right.ase"));
    private static Aseprite playerWalkLeft = playerWalkLeft = new Aseprite(Path.Combine("Assets", "Player", "player_walk_left.ase"));

    float speed = 300;
    float jumpHeight = 400;

    PreviousFacing previousFacing;

    public AnimationManager AnimationManager { get; } = new();

    public Player(Vector2 position) : base(position, 0, Vector2.One * Game.Scale, new Vector2(playerIdleLeft.Width, playerIdleLeft.Height))
    {
        AnimationManager.AddAnimation("player_idle_left", new Animation(playerIdleLeft, playerIdleLeft.Width, playerIdleLeft.Height, 0.5f, true));
        AnimationManager.AddAnimation("player_idle_right", new Animation(playerIdleRight, playerIdleRight.Width, playerIdleRight.Height, 0.5f, true));

        AnimationManager.AddAnimation("player_walk_right", new Animation(playerWalkRight, playerWalkRight.Width, playerWalkRight.Height, 0.25f, true));
        AnimationManager.AddAnimation("player_walk_left", new Animation(playerWalkLeft, playerWalkLeft.Width, playerWalkLeft.Height, 0.25f, true));

        previousFacing = PreviousFacing.Right;
    }

    public override void Update()
    {
        if (Input.Keyboard.Down(Keys.D))
        {
            if (IsOnGroundCoyote)
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

            velocity.X = speed;
            previousFacing = PreviousFacing.Right;
        }
        else if (Input.Keyboard.Down(Keys.A))
        {
            if (IsOnGroundCoyote)
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

            velocity.X = -speed;
            previousFacing = PreviousFacing.Left;
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
            velocity.X = 0;
        }

        if (Input.Keyboard.Pressed(Keys.Space) && IsOnGroundCoyote)
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
