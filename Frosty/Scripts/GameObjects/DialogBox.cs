using Foster.Framework;
using Frosty.Scripts.Core;
using System.Numerics;
using Timer = Frosty.Scripts.Components.Timer;

namespace Frosty.Scripts.GameObjects;

public class DialogBox : GameObject
{
    public bool Finished { get; private set; }
    public string CurrentSentence { get; private set; }

    float CharPerSeconds { get; set; }


    Timer speakDelayTimer;

    string[] dialog;

    bool hide = true;
    bool isNextSentence;
    string sentenceSpeak = "";
    int sentenceIndex;
    int dialogIndex;
    float upAndDownArrow;

    public event Action? DialogFinished;

    public DialogBox(Vector2 position, float charPerSeconds) : base(position, 0, Vector2.One * Game.Scale, new Vector2(180, 42))
    {
        CharPerSeconds = charPerSeconds;

        speakDelayTimer = new Timer(0, false);
        speakDelayTimer.OnTimeout += () =>
        {
            if (sentenceIndex < CurrentSentence.Length)
            {
                Game.PlayerTalk.Play();

                sentenceSpeak += CurrentSentence[sentenceIndex];
            }
            sentenceIndex++;
        };
    }

    public void Play(string[] dialog)
    {
        Finished = false;
        this.dialog = dialog;

        dialogIndex = 0;
        sentenceIndex = 0;
        sentenceSpeak = "";
        hide = false;
        CurrentSentence = dialog[dialogIndex];

        speakDelayTimer.WaitTime = CurrentSentence.Length / (CurrentSentence.Length * CharPerSeconds);
        speakDelayTimer.Start();
    }

    public override void Update()
    {
        upAndDownArrow = (float)Math.Sin(Time.Duration.TotalSeconds * 5);

        speakDelayTimer.Update();

        if (Input.Keyboard.Pressed(Keys.Enter) && isNextSentence)
        {
            dialogIndex++;

            if (dialogIndex > dialog.Length - 1)
            {
                speakDelayTimer.Stop();
                DialogFinished?.Invoke();
                hide = true;
                Finished = true;
                return;
            }

            sentenceIndex = 0;
            sentenceSpeak = "";

            CurrentSentence = dialog[dialogIndex];
            speakDelayTimer.WaitTime = CurrentSentence.Length / (CurrentSentence.Length * CharPerSeconds);

            isNextSentence = false;
        }

        if (CurrentSentence is not null)
        {
            if (sentenceIndex > CurrentSentence.Length)
            {
                isNextSentence = true;
            }
        }
    }

    public override void Draw(Batcher batcher)
    {
        if (!hide)
        {
            batcher.PushMatrix(position, scale, size / 2, rotation);
            batcher.Image(Game.dialogBoxTexture, Color.White);
            batcher.PopMatrix();

            batcher.Text(Game.M5x7Dialog, sentenceSpeak, position - new Vector2(Game.dialogBoxTexture.Width * Game.Scale / 2 - 25, Game.dialogBoxTexture.Height * Game.Scale / 2 - 15), Color.White);

            if (isNextSentence)
            {
                batcher.Triangle(new Vector2(position.X - 15, position.Y + Game.dialogBoxTexture.Height * Game.Scale / 2 - 10 + upAndDownArrow),
                                 new Vector2(position.X + 15, position.Y + Game.dialogBoxTexture.Height * Game.Scale / 2 - 10 + upAndDownArrow),
                                 new Vector2(position.X, position.Y + 15 + Game.dialogBoxTexture.Height * Game.Scale / 2 - 10 + upAndDownArrow),
                                 Color.White);

                batcher.TriangleLine(new Vector2(position.X - 15, position.Y + Game.dialogBoxTexture.Height * Game.Scale / 2 - 10 + upAndDownArrow),
                                     new Vector2(position.X + 15, position.Y + Game.dialogBoxTexture.Height * Game.Scale / 2 - 10 + upAndDownArrow),
                                     new Vector2(position.X, position.Y + 15 + Game.dialogBoxTexture.Height * Game.Scale / 2 - 10 + upAndDownArrow),
                                     1,
                                     Color.Black);
            }

            base.Draw(batcher);
        }
    }

    public override void Dispose()
    {

    }
}
