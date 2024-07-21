using Foster.Framework;
using Frosty.Scripts.Core;
using System.Numerics;
using Timer = Frosty.Scripts.Components.Timer;

namespace Frosty.Scripts.GameObjects.Effects;

public class HouseChimneySmokeSpawner
{
    public Vector2 Position;
    public float SpawnInterval { get; }
    public float Wide { get; set; }
    public List<HouseChimneySmoke> houseChimneySmokes = new();

    Timer timer;

    public HouseChimneySmokeSpawner(Vector2 position, float interval, float wide)
    {
        Position = position;
        Wide = wide;
        SpawnInterval = interval;

        timer = new Timer(SpawnInterval, false);


        for (int i = 0; i < 100; i++)
        {
            houseChimneySmokes.Add(new HouseChimneySmoke(Position + new Vector2(Game.Random.Next((int)Position.X, (int)(Position.X + Wide)), Game.Random.Next((int)Position.Y - 300, (int)(Position.Y - 100))), Game.Random.Next(2, 3), 2000));
        }

        timer.OnTimeout += () =>
        {
            houseChimneySmokes.Add(new HouseChimneySmoke(Position + new Vector2(Game.Random.Next((int)Position.X, (int)(Position.X + Wide)), -10), Game.Random.Next(2, 3), 2000));
        };

        timer.Start();
    }

    public void Update()
    {
        timer.Update();

        foreach (var houseChimneySmoke in houseChimneySmokes)
        {
            houseChimneySmoke.Update();
        }

        for (int i = houseChimneySmokes.Count - 1; i >= 0; i--)
        {
            if (houseChimneySmokes[i].position.Y < -20)
            {
                houseChimneySmokes.RemoveAt(i);
            }
        }
    }

    public void Draw(Batcher batcher)
    {
        foreach (var houseChimneySmoke in houseChimneySmokes)
        {
            houseChimneySmoke.Draw(batcher);
        }
    }
}
