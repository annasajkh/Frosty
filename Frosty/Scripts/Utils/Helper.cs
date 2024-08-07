﻿using Foster.Framework;
using Frosty.Scripts.Components;
using Frosty.Scripts.GameObjects.StaticObjects;
using Frosty.Scripts.GameObjects.StaticTiles;
using System.Numerics;

namespace Frosty.Scripts.Utils;

public static class Helper
{
    public static float SnapToGrid(float value, float gridSize)
    {
        return (float)(MathF.Floor(value / gridSize) * gridSize);
    }

    public static Vector2 SnapToGrid(Vector2 value, int gridSize)
    {
        return new Vector2(SnapToGrid(value.X, gridSize), SnapToGrid(value.Y, gridSize));
    }

    public static Vector2i SnapToGrid(Vector2i value, int gridSize)
    {
        return new Vector2i((int)SnapToGrid(value.X, gridSize), (int)SnapToGrid(value.Y, gridSize));
    }

    public static void DrawTextCentered(string text, Vector2 position, Color color, SpriteFont font, Batcher batcher)
    {
        batcher.PushMatrix(position, Vector2.One, new Vector2(font.WidthOf(text) / 2, font.HeightOf(text) / 2), 0);
        batcher.Text(font, text, Vector2.Zero, color);
        batcher.PopMatrix();
    }

    public static TileType TileObjectToTileType(TileObject tileObject)
    {
        switch (tileObject)
        {
            case Solid:
                return TileType.Solid;

            case Spike:
                return TileType.Spike;

            case Ice:
                return TileType.Ice;
            
            case BrittleIce:
                return TileType.BrittleIce;

            case Decoration:
                return TileType.Decoration;

            default:
                throw new Exception("Not Implemented yet");
        }
    }

    public static TileObject CreateTileObjectUsingTileType(Vector2 position, Vector2 scale, Texture texture, Rect rect, TileType tileType)
    {
        switch (tileType)
        {
            case TileType.Solid:
                return new Solid(position, scale, texture, rect);

            case TileType.Spike:
                return new Spike(position, scale, texture, rect);

            case TileType.Ice:
                return new Ice(position, scale, texture, rect);

            case TileType.BrittleIce:
                return new BrittleIce(position, scale, texture, rect);

            case TileType.Decoration:
                return new Decoration(position, scale, texture, rect);

            default:
                throw new Exception("Not Implemented yet");
        }
    }

    public static bool IsOverlapOnGround(Rect first, Rect second)
    {
        if (!first.Overlaps(second))
        {
            return false;
        }

        Rect collisionResult = first.OverlapRect(second);

        if (collisionResult.Width > collisionResult.Height && first.Y < second.Y)
        {
            return true;
        }

        return false;
    }
}