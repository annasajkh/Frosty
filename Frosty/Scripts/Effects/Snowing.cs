using Foster.Framework;
using Frosty.Scripts.Core;
using System.Numerics;
using Timer = Frosty.Scripts.Components.Timer;

namespace Frosty.Scripts.Effects;

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

        for (int i = 0; i < 200; i++)
        {
            Snows.Add(new Snow(Position + new Vector2(Game.Random.Next((int)Position.X, (int)(Position.X + Wide)), Game.Random.Next((int)Position.Y, (int)(Position.Y + Wide))), Game.Random.Next(1, 2), Game.Random.Next(50, 200)));
        }

        timer.OnTimeout += () =>
        {
            Snows.Add(new Snow(Position + new Vector2(Game.Random.Next((int)Position.X, (int)(Position.X + Wide)), -10), Game.Random.Next(1, 2), Game.Random.Next(50, 200)));
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
            if (Snows[i].position.Y > App.Height + 10)
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
