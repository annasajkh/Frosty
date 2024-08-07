﻿using Foster.Framework;

namespace Frosty.Scripts.Components;

/// <summary>
/// Sprite animation
/// </summary>
public sealed class Animation
{
    public Aseprite Aseprite { get; }

    public float PlaySpeed { get; }
    public bool Playing { get; private set; }
    public bool Looping { get;  }

    public int FrameIndex { get; set; }
    public Texture CurrentFrame { get; private set; }

    public int TotalFrames
    {
        get
        {
            return Aseprite.Frames.Length;
        }
    }

    private float time;

    public Animation(Aseprite aseprite, int frameWidth, int frameHeight, float playSpeed, bool looping)
    {
        PlaySpeed = playSpeed;
        Aseprite = aseprite;
        Looping = looping;

        CurrentFrame = new Texture(frameWidth, frameHeight, TextureFormat.R8G8B8A8);
    }

    public void Play()
    {
        Playing = true;
    }

    public void Stop()
    {
        Playing = false;
    }

    public void Update()
    {
        CurrentFrame.SetData<Color>(Aseprite.RenderFrame(FrameIndex).Data);

        if (Playing && time > PlaySpeed / Aseprite.Frames.Length)
        {
            FrameIndex++;
            time = 0;
        }

        if (FrameIndex > Aseprite.Frames.Length - 1)
        {
            if (Looping)
            {
                FrameIndex = 0;
            }
            else
            {
                FrameIndex = Aseprite.Frames.Length - 1;
            }
        }

        time += Time.Delta;
    }
}