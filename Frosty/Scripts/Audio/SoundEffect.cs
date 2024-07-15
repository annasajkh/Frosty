using Frosty.Scripts.Bindings.SDL2;

namespace Frosty.Scripts.Audio;


/// <summary>
/// There can be multiple sound effect playing at once, this represent SDL2_Mixer "chunk", which is intended to plays short audio
/// </summary>
public class SoundEffect : Audio, IDisposable
{
    public static SoundEffect Load(string filePath, int volume = 10, bool loop = false)
    {
        return new SoundEffect(SDL_mixer.Mix_LoadWAV(filePath), volume, loop);
    }

    public static string GetChunkDecoder(int index)
    {
        return SDL_mixer.Mix_GetChunkDecoder(index);
    }

    public static int GetNumChunkDecoders()
    {
        return SDL_mixer.Mix_GetNumChunkDecoders();
    }

    public SoundEffect(nint handle, int volume, bool loop) : base(handle, volume, loop)
    {

    }

    public void Dispose()
    {
        SDL_mixer.Mix_FreeChunk(Handle);
    }
}