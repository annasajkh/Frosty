namespace Frosty.Scripts.Components.Audio;

public enum AudioType
{
    Mp3,
    Ogg,
    Wav
}


public class Audio
{
    public nint Handle { get; private set; }
    public int Volume { get; set; }
    public bool Loop { get; set; }

    public Audio(nint handle, int volume, bool loop)
    {
        Handle = handle;
        Volume = volume;
        Loop = loop;
    }
}
