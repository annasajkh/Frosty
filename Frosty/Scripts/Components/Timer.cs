using Foster.Framework;

namespace Frosty.Scripts.Components;

public sealed class Timer
{
    public float WaitTime { get; set; }
    public bool Oneshot { get; set; }
    public float TimeLeft { get; private set; }
    public bool Paused { get; private set; }

    public event Action? OnTimeout;

    public Timer(float waitTime, bool isOneshot)
    {
        WaitTime = waitTime;
        Oneshot = isOneshot;
    }

    public void Start()
    {
        Paused = false;
    }

    public void Stop()
    {
        Paused = true;
    }

    public void Update()
    {
        if (!Paused)
        {
            TimeLeft += Time.Delta;

            if (TimeLeft >= WaitTime)
            {
                TimeLeft = 0;

                if (OnTimeout != null)
                {
                    OnTimeout();
                }

                if (Oneshot)
                {
                    Paused = true;
                }
            }
        }
    }
}