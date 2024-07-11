﻿using Foster.Framework;
using Frosty.Scripts.Core;
using Frosty.Scripts.DataStructures;
using Frosty.Scripts.Entities;
using System.Numerics;

namespace Frosty.Scripts.StaticObjects;

public class TileObject : Entity
{
    public Texture Texture { get; }
    public Rect Rect { get; }

    public TileObject(Vector2 position, Vector2 scale, Texture texture, Rect rect) : base(position, 0, scale, Vector2.One * Game.TileSize)
    {
        Texture = texture;
        Rect = rect;
    }

    public void Draw(Batcher batcher)
    {
        batcher.PushMatrix(position, scale, size / 2, rotation);
        batcher.Image(Texture, Rect, Vector2.Zero, Color.White);
        batcher.PopMatrix();
    }

    public override int GetHashCode()
    {
        return new Vector2i((int)position.X, (int)position.Y).GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is TileObject other)
        {
            return new Vector2i((int)position.X, (int)position.Y).Equals(new Vector2i((int)other.position.X, (int)other.position.Y));
        }
        return false;
    }
}
