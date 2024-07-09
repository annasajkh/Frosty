using Foster.Framework;
using Frosty.Scripts.Core;
using System.Numerics;

namespace Frosty.Scripts.Components;

public class Snowing
{
    public Vector2 Position;
    public float SpawnInterval { get; } // in seconds
    public float Wide { get; set; }
    public List<Snow> Snows = new();

    Timer timer;

    public Snowing(Vector2 position, float interval, float wide)
    {
        Position = position;
        Wide = wide;
        SpawnInterval = interval;

        timer = new Timer(SpawnInterval, false);

        timer.OnTimeout += () =>
        {
            Snows.Add(new Snow(Position + new Vector2(Application.Random.Next((int)(Position.X), (int)(Position.X + Wide)), -10), Application.Random.Next(1, 3), Application.Random.Next(50, 200)));
        };

        timer.Start();
    }

    public void Update()
    {
        timer.Update();

        foreach (var snow in Snows)
        {
            snow.Update();
        }

        for (int i = Snows.Count - 1; i >= 0; i--)
        {
            if (Snows[i].Position.Y > App.Height + 10)
            {
                Snows.RemoveAt(i);
            }
        }
    }

    public void Draw(Batcher batcher)
    {
        foreach (var snow in Snows)
        {
            snow.Draw(batcher);
        }
    }
}
