﻿using Foster.Framework;
using Frosty.Scripts.Abstracts;
using Frosty.Scripts.Core;
using System.Numerics;
using Timer = Frosty.Scripts.Components.Timer;
using Color = Foster.Framework.Color;

namespace Frosty.Scripts.Effects;

public class PlayerDieParticle : GameObject
{
    public bool Destroyed { get; private set; }

    float speed;
    float rotationDir;

    Timer destroyTimer;
    Vector2 velocity;

    public PlayerDieParticle(Vector2 position, Vector2 size, float lifeTime, float speed) : base(position, 0, Vector2.One, size)
    {
        this.speed = speed;

        destroyTimer = new Timer(lifeTime, true);
        destroyTimer.Start();
        destroyTimer.OnTimeout += () =>
        {
            Destroyed = true;
        };

        velocity = new Vector2(1, 0);
        velocity = Vector2.Transform(velocity, Matrix3x2.CreateRotation(Game.Random.NextSingle() * MathF.Tau)) * speed;
        rotationDir = Game.Random.NextSingle() > 0.5f ? 1 : -1;
    }

    public void Update()
    {
        destroyTimer.Update();

        position += velocity * Time.Delta;
        rotation += rotationDir * Time.Delta * 10;
    }

    public void Draw(Batcher batcher)
    {
        batcher.PushMatrix(position, Vector2.One * 3, new Vector2(size.X, size.Y) / 2, rotation);
        batcher.Rect(Vector2.Zero, Vector2.One * size, Color.White);
        batcher.PopMatrix();
    }
}
