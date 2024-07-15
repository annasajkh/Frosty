using Frosty.Scripts.Bindings.SDL2;

namespace Frosty.Scripts.Audio;

/// <summary>
/// Something that play SoundEffect
/// </summary>
public class SoundEffectPlayer
{
    public SoundEffect? SoundEffect { get; private set; }

    public void SetSource(SoundEffect soundEffect)
    {
        SoundEffect = soundEffect;
    }

    public void Play()
    {
        if (SoundEffect is SoundEffect soundEffect)
        {
            int volumeError = SDL_mixer.Mix_VolumeChunk(soundEffect.Handle, soundEffect.Volume);
            if (volumeError == 0)
            {
                throw new Exception("Cannot set volume");
            }

            SDL_mixer.Mix_PlayChannel(-1, soundEffect.Handle, soundEffect.Loop ? -1 : 0);
        }
    }
}
