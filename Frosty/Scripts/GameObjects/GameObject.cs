using Foster.Framework;
using Frosty.Scripts.Core;
using Frosty.Scripts.Interfaces;
using System.Numerics;

namespace Frosty.Scripts.GameObjects;

/// <summary>
/// Anything that can exist in the world is a GameObject
/// </summary>
public abstract class GameObject : IDrawable, IUpdateable, IDisposable
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

    public abstract void Update();

    public virtual void Draw(Batcher batcher)
    {
        if (Game.ShowColliders)
        {
            batcher.RectLine(BoundingBox, 1, Color.Green);
        }
    }

    public abstract void Dispose();
}