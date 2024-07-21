using Foster.Framework;
using Frosty.Scripts.Components;
using Frosty.Scripts.Core;
using Frosty.Scripts.GameObjects.Effects;
using Frosty.Scripts.Utils;
using Newtonsoft.Json;
using System.Numerics;

namespace Frosty.Scripts.Scenes;

public class MainMenu : Scene
{
    protected SnowSpawner snowSpawner;


    public override void Startup()
    {
        if (File.Exists("Save.json"))
        {
            SaveData saveData = JsonConvert.DeserializeObject<SaveData>(File.ReadAllText("Save.json"));

            Game.PlayerLevel = saveData.level;
            Game.PlayerDeath = saveData.playerDeath;
            Game.GameRunTime = saveData.gameRunTime;
        }
        else
        {
            Game.PlayerLevel = "IntroLevel";
            Game.PlayerDeath = 0;
            Game.GameRunTime = 0;
        }

        snowSpawner = new SnowSpawner(new Vector2(0, 0), 0.005f, App.Width);
    }

    public override void Update()
    {
        if (Input.Keyboard.Pressed(Keys.Enter))
        {
            if (Game.PlayerLevel is not null)
            {
                Game.SceneManager.ChangeScene(Game.PlayerLevel);
            }
            else
            {
                Game.SceneManager.ChangeScene("IntroLevel");
            }

        }

        snowSpawner.Update();
    }

    public override void Render(Batcher batcher)
    {
        Graphics.Clear(Color.CornflowerBlue);

        batcher.Image(Game.NightSky, Vector2.Zero, Color.White);
        snowSpawner.Draw(batcher);

        Helper.DrawTextCentered("Press Enter to Start", new Vector2(App.Width / 2, App.Height / 2), Color.White, Game.M5x7Menu, batcher);
    }

    public override void Shutdown()
    {

    }
}
