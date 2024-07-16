using Foster.Framework;
using Frosty.Scripts.Core;
using System.Numerics;

namespace Frosty.Scripts.GameObjects;

/// <summary>
/// Anything that can exist in the world is a GameObject
/// </summary>
public class GameObject
{
    public Vector2 position;
    public float rotation;
    public Vector2 scale;
    public Vector2 size;

    /// <summary>
    /// The rectangle that cover the entire object
    /// </summary>
    public virtual Rect BoundingBox
    {
        get
        {
            return new Rect(position - scale * size / 2, position + scale * size / 2);
        }
    }

    public GameObject(Vector2 position, float rotation, Vector2 scale, Vector2 size)
    {
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
        this.size = size;
    }

    public virtual void Draw(Batcher batcher)
    {
        if (Game.DebugMode)
        {
            batcher.RectLine(BoundingBox, 1, Color.Green);
        }
    }
}