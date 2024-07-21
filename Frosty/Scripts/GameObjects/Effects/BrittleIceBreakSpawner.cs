using Foster.Framework;
using Frosty.Scripts.Core;
using System.Numerics;
using Timer = Frosty.Scripts.Components.Timer;

namespace Frosty.Scripts.GameObjects.Effects;

public class BrittleIceBreakSpawner
{
    public Vector2 Position;
    public List<BrittleIceBreak> brittleIceBreaks = new();
    public bool Deleted { get; private set; }

    Vector2 size;
    Timer deleteTimer;

    public BrittleIceBreakSpawner(Vector2 position, Vector2 size)
    {
        Position = position;
        this.size = size;

        deleteTimer = new Timer(5, true);
        deleteTimer.OnTimeout += () =>
        {
            Deleted = true;
        };

        deleteTimer.Start();

        for (int i = 0; i < 100; i++)
        {
            brittleIceBreaks.Add(new BrittleIceBreak(Position + new Vector2(Game.Random.Next((int)(Position.X - size.X / 2), (int)(Position.X + size.X / 2)), Game.Random.Next((int)(Position.Y - size.Y / 2), (int)(Position.Y + size.Y / 2))), Game.Random.Next(2, 3)));
        }
    }

    public void Update()
    {
        deleteTimer.Update();

        foreach (var brittleIceBreak in brittleIceBreaks)
        {
            brittleIceBreak.Update();
        }

        for (int i = brittleIceBreaks.Count - 1; i >= 0; i--)
        {
            if (brittleIceBreaks[i].position.Y > App.Height + 10)
            {
                brittleIceBreaks.RemoveAt(i);
            }
        }
    }

    public void Draw(Batcher batcher)
    {
        foreach (var brittleIceBreak in brittleIceBreaks)
        {
            brittleIceBreak.Draw(batcher);
        }
    }
}
