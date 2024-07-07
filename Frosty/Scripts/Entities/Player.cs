using Foster.Framework;
using System.Numerics;

namespace Frosty.Scripts.Entities;

public class Player : Entity
{
    float speed = 500;
    float jumpHeight = 500;

    public Player(Vector2 position) : base(position, 0, Vector2.One, Vector2.One * 50)
    {

    }

    public override void Update()
    {
        if (Input.Keyboard.Down(Keys.D))
        {
            velocity.X = speed;
        }
        else if (Input.Keyboard.Down(Keys.A))
        {
            velocity.X = -speed;
        }
        else
        {
            velocity.X = 0;
        }

        if (Input.Keyboard.Pressed(Keys.Space) && IsOnGround)
        {
            velocity.Y = -jumpHeight;
        }

        base.Update();
    } 

    public void Draw(Batcher batcher)
    {
        batcher.PushMatrix(position, scale, size / 2, rotation);
        batcher.Rect(Vector2.Zero, size, Color.Blue);
        batcher.PopMatrix();
    }
}
