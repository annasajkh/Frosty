using Foster.Framework;
using Frosty.Scripts.Core;
using Frosty.Scripts.GameObjects;
using Frosty.Scripts.Utils;
using System.Numerics;

namespace Frosty.Scripts.UIs;

public class Button : GameObject
{
    public string Text { get; set; }
    public Color TextColor { get; set; }
    public Color ButtonColor { get; set; }
    public Color ButtonNormalColor { get; set; }
    public Color ButtonHoverColor { get; set; }
    public event Action? OnPressed;

    public Button(string text, Color textColor, Color buttonColor, Color buttonHoverColor, Vector2 position, Vector2 size) : base(position, 0, Vector2.One, size)
    {
        Text = text;
        TextColor = textColor;
        ButtonColor = buttonColor;
        ButtonHoverColor = buttonHoverColor;
    }

    public override void Update()
    {
        if (BoundingBox.Contains(Input.Mouse.Position))
        {
            ButtonNormalColor = ButtonHoverColor;
        }
        else
        {
            ButtonNormalColor = ButtonColor;
        }

        if (BoundingBox.Contains(Input.Mouse.Position) && Input.Mouse.Pressed(MouseButtons.Left))
        {
            OnPressed?.Invoke();
        }
    }

    public override void Draw(Batcher batcher)
    {
        batcher.PushMatrix(position, scale, size / 2, rotation);
        batcher.Rect(Vector2.Zero, size, ButtonNormalColor);
        Helper.DrawTextCentered(Text, size / 2, TextColor, Game.M5x7Dialog, batcher);
        batcher.PopMatrix();

        base.Draw(batcher);
    }

    public override void Dispose()
    {

    }
}
