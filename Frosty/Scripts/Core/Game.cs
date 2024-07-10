using Foster.Framework;
using Frosty.Scripts.Managers;
using Frosty.Scripts.Scenes;

namespace Frosty.Scripts.Core;

public sealed class Game : Module
{
    public static float gravity = 20;
    public static Random Random { get; } = new(Time.Now.Milliseconds);
    public static SceneManager SceneManager { get; private set; } = new(initialSceneName: "MainMenu", initialScene: new MainMenu());

    Batcher batcher = new();

    public override void Startup()
    {
        SceneManager.AddScene("TestLevel", new TestLevel());
        SceneManager.ChangeScene("TestLevel");
    }

    public override void Update()
    {
        SceneManager.ActiveScene?.Update();
    }

    public override void Render()
    {
        SceneManager.ActiveScene?.Render(batcher);

        batcher.Render();
        batcher.Clear();
    }

    public override void Shutdown()
    {
        SceneManager.ActiveScene?.Shutdown();
    }
}
