namespace Frosty.Scripts.Components;

public sealed class AnimationManager
{
    private Dictionary<string, Animation> animations = new();

    private string? currentAnimation;

    public Animation? CurrentAnimation
    {
        get
        {
            if (currentAnimation is null)
            {
                return null;
            }

            return animations[currentAnimation];
        }
    }

    public void SetCurrent(string name)
    {
        if (!animations.ContainsKey(name))
        {
            throw new Exception($"animations doesn't contain {name}");
        }

        currentAnimation = name;
    }

    public void AddAnimation(string name, Animation animation)
    {
        animations.Add(name, animation);
    }

    public void RemoveAnimation(string name)
    {
        animations.Remove(name);
    }


    public void Play()
    {
        if (currentAnimation is null)
        {
            return;
        }

        animations[currentAnimation].Play();
    }

    public void Stop()
    {
        if (currentAnimation is null)
        {
            return;
        }

        animations[currentAnimation].Stop();
    }

    public void Update()
    {
        if (currentAnimation is null)
        {
            return;
        }

        animations[currentAnimation].Update();
    }
}
