using Foster.Framework;
using Frosty.Scripts.Core;
using Frosty.Scripts.DataStructures;
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
        batcher.PushMatrix(position, Vector2.One, new Vector2(Game.ArialFont.WidthOf(text) / 2, Game.ArialFont.HeightOf(text) / 2), 0);
        batcher.Text(font, text, Vector2.Zero, color);
        batcher.PopMatrix();
    }
}