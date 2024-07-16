using Foster.Framework;
using Frosty.Scripts.Bindings.SDL2;
using Frosty.Scripts.Core;

namespace Frosty.Scripts;

internal static class Program
{
    static void Main(string[] args)
    {
        if (SDL.SDL_Init(SDL.SDL_INIT_AUDIO) < 0)
        {
            throw new Exception("Cannot initialize SDL Audio");
        }

        SDL_mixer.Mix_OpenAudio(frequency: 44100, format: SDL.AUDIO_S16SYS, channels: 2, chunksize: 2048);

#if DEBUG
        App.Resizable = true;
#else
        App.Resizable = false;
#endif
        App.Register<Game>();

        App.Run("Frosty", 960, 528);
    }
}