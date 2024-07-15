using Foster.Framework;
using Frosty.Scripts.Abstracts;
using Frosty.Scripts.Core;
using Frosty.Scripts.Utils;
using System.Numerics;

namespace Frosty.Scripts.Components;

public class DialogBox : GameObject
{
    public string CurrentSentence { get; private set; }
    
    float CharPerSeconds { get; set; }
    float BetweenSentenceDelay { get; set; }

    static Texture dialogBoxTexture = new Texture(new Aseprite(Path.Combine("Assets", "UIs", "dialog_box.ase")).Frames[0].Cels[0].Image);
    
    Timer speakDelayTimer;

    string[] dialog;
    
    bool hide = true;
    bool isNextSentence;
    string sentenceSpeak = "";
    int sentenceIndex;
    int dialogIndex;
    float upAndDownArrow;

    public event Action? DialogFinished;

    public DialogBox(Vector2 position, float charPerSeconds, float betweenSentenceDelay) : base(position, 0, Vector2.One * 3, new Vector2(dialogBoxTexture.Width, dialogBoxTexture.Height))
    {
        CharPerSeconds = charPerSeconds;
        BetweenSentenceDelay = betweenSentenceDelay;

        speakDelayTimer = new Timer(0, false);
        speakDelayTimer.OnTimeout += () =>
        {
            if (sentenceIndex < CurrentSentence.Length)
            {
                Game.SoundEffectPlayer.SetSource(Game.PlayerTalk);
                Game.SoundEffectPlayer.Play();

                sentenceSpeak += CurrentSentence[sentenceIndex];
            }
            sentenceIndex++;
        };
    }

    public void Play(string[] dialog)
    {
        this.dialog = dialog;

        dialogIndex = 0;
        sentenceIndex = 0;
        sentenceSpeak = "";
        hide = false;
        CurrentSentence = dialog[dialogIndex];

        speakDelayTimer.WaitTime = CurrentSentence.Length / (CurrentSentence.Length * CharPerSeconds);
        speakDelayTimer.Start();
    }

    public void Update()
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
            batcher.Image(dialogBoxTexture, Color.White);
            batcher.PopMatrix();

            Helper.DrawTextCentered(sentenceSpeak, position, Color.White, Game.M5x7Dialog, batcher);

            if (isNextSentence)
            {                
                batcher.Triangle(new Vector2(position.X - 15, position.Y + dialogBoxTexture.Height * 3 / 2 - 10 + upAndDownArrow), 
                                 new Vector2(position.X + 15, position.Y + dialogBoxTexture.Height * 3 / 2 - 10 + upAndDownArrow), 
                                 new Vector2(position.X, position.Y + 15 + dialogBoxTexture.Height * 3 / 2 - 10 + upAndDownArrow), 
                                 Color.White);

                batcher.TriangleLine(new Vector2(position.X - 15, position.Y + dialogBoxTexture.Height * 3 / 2 - 10 + upAndDownArrow),
                                     new Vector2(position.X + 15, position.Y + dialogBoxTexture.Height * 3 / 2 - 10 + upAndDownArrow),
                                     new Vector2(position.X, position.Y + 15 + dialogBoxTexture.Height * 3 / 2 - 10 + upAndDownArrow),
                                     1,
                                     Color.Black);
            }

            base.Draw(batcher);
        }
    }
}
