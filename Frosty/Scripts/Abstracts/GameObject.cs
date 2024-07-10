using Foster.Framework;
using System.Numerics;

namespace Frosty.Scripts.Abstracts;

public class GameObject
{
    public Vector2 position;
    public float rotation;
    public Vector2 scale;
    public Vector2 size;

    public Rect Rect
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
}