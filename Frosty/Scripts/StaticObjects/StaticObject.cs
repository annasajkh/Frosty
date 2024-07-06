using Frosty.Scripts.Abstracts;
using System.Numerics;

namespace Frosty.Scripts.StaticObjects;

public class StaticObject : GameObject2D
{
    public StaticObject(Vector2 position) : base(position, 0, Vector2.One, Vector2.Zero)
    {

    }
}
