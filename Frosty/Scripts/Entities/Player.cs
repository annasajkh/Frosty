using Foster.Framework;
using System.Numerics;

namespace Frosty.Scripts.Entities;

public class Player : Entity
{
    float speed = 500;
    float jumpHeight = 700;

    public Player(Vector2 position) : base(position)
    {
        size = Vector2.One * 50;
    }

    public override void Update()
    {
        float horizontalMovement = 0;

        if (Input.Keyboard.Down(Keys.D))
        {
            horizontalMovement = speed;
        }
        else if (Input.Keyboard.Down(Keys.A))
        {
            horizontalMovement = -speed;
        }

        if (Input.Keyboard.Pressed(Keys.Space))
        {
            velocity.Y = -jumpHeight;
        }

        velocity.X = horizontalMovement;

        base.Update();
    }

    public void Draw(Batcher batcher)
    {
        batcher.PushMatrix(position, scale, size / 2, rotation);
        batcher.Rect(Vector2.Zero, size, Color.Blue);
        batcher.PopMatrix();
    }
}
