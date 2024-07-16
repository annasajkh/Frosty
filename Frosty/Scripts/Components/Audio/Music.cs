using Frosty.Scripts.Bindings.SDL2;

namespace Frosty.Scripts.Components.Audio;

/// <summary>
/// There can be only one music playing at the time, this represent SDL2_Mixer music which there is can only be 1 music playing at the time
/// </summary>
public class Music : Audio, IDisposable
{
    public static nint GetHookData()
    {
        return SDL_mixer.Mix_GetMusicHookData();
    }

    public static string GetDecoder(int index)
    {
        return SDL_mixer.Mix_GetMusicDecoder(index);
    }

    public static int GetNumDecoders()
    {
        return SDL_mixer.Mix_GetNumMusicDecoders();
    }

    public static void SetPosition(double position)
    {
        int musicPositionError = SDL_mixer.Mix_SetMusicPosition(position);

        if (musicPositionError == 0)
        {
            throw new Exception("Cannot set position");
        }
    }

    public static void FadeOut(int ms)
    {
        int error = SDL_mixer.Mix_FadeOutMusic(ms);

        if (error == 0)
        {
            throw new Exception("Cannot Fade Out Music");
        }
    }

    public static Music Load(string filePath, AudioType audioType, int volume = 10, bool loop = false)
    {
        SDL_mixer.MIX_InitFlags mixerInitFlag;

        switch (audioType)
        {
            case AudioType.Mp3:
                mixerInitFlag = SDL_mixer.MIX_InitFlags.MIX_INIT_MP3;
                break;
            case AudioType.Ogg:
                mixerInitFlag = SDL_mixer.MIX_InitFlags.MIX_INIT_OGG;
                break;
            case AudioType.Wav:
                return new Music(SDL_mixer.Mix_LoadMUS(filePath), audioType, volume, loop);
            default:
                throw new Exception("Music Type is not supported");
        }

        if (SDL_mixer.Mix_Init(mixerInitFlag) == 0)
        {
            throw new Exception($"Cannot initialize SDL Mixer Music {SDL_mixer.Mix_GetError()}");
        }

        return new Music(SDL_mixer.Mix_LoadMUS(filePath), audioType, volume, loop);
    }

    public AudioType AudioType { get; }

    public double Duration
    {
        get
        {
            return SDL_mixer.Mix_MusicDuration(Handle);
        }
    }

    public Music(nint handle, AudioType audioType, int volume, bool loop) : base(handle, volume, loop)
    {
        AudioType = audioType;
    }

    public void FadeIn(int loops, int ms)
    {
        int error = SDL_mixer.Mix_FadeInMusic(Handle, loops, ms);

        if (error == 0)
        {
            throw new Exception("Cannot Fade in Music");
        }
    }

    public void FadeInPosition(int loops, int ms, double position)
    {
        int error = SDL_mixer.Mix_FadeInMusicPos(Handle, loops, ms, position);

        if (error == 0)
        {
            throw new Exception("Cannot Fade In Music Position Music");
        }
    }

    public string AlbumTag()
    {
        return SDL_mixer.Mix_GetMusicAlbumTag(Handle);
    }

    public string ArtistTag()
    {
        return SDL_mixer.Mix_GetMusicArtistTag(Handle);
    }

    public string CopyrightTag()
    {
        return SDL_mixer.Mix_GetMusicCopyrightTag(Handle);
    }

    public double GetLoopEndTime()
    {
        return SDL_mixer.Mix_GetMusicLoopEndTime(Handle);
    }

    public double GetLoopLengthTime()
    {
        return SDL_mixer.Mix_GetMusicLoopLengthTime(Handle);
    }

    public double GetLoopStartTime()
    {
        return SDL_mixer.Mix_GetMusicLoopStartTime(Handle);
    }

    public double GetPosition()
    {
        return SDL_mixer.Mix_GetMusicPosition(Handle);
    }

    public string GetTitle()
    {
        return SDL_mixer.Mix_GetMusicTitle(Handle);
    }

    public string GetTitleTag()
    {
        return SDL_mixer.Mix_GetMusicTitleTag(Handle);
    }

    public int GetVolumeStream()
    {
        return SDL_mixer.Mix_GetVolumeMusicStream(Handle);
    }

    public void Hook(SDL_mixer.MixFuncDelegate mix_func, nint arg)
    {
        SDL_mixer.Mix_HookMusic(mix_func, arg);
    }

    public void Dispose()
    {
        SDL_mixer.Mix_FreeMusic(Handle);
    }
}