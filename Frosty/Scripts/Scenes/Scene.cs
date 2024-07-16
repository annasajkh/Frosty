using Foster.Framework;

namespace Frosty.Scripts.Scenes;

/// <summary>
/// Scene is a place where things happened like main menu or the game world
/// </summary>
public abstract class Scene
{
    public abstract void Startup();
    public abstract void Update();
    public abstract void Render(Batcher batcher);
    public abstract void Shutdown();

    public void StartupInternal()
    {
        Startup();
        Log.Info($"{GetType().Name} Scene is Loaded");
    }

    public void ShutdownInternal()
    {
        Shutdown();
        Log.Info($"{GetType().Name} Scene is Unloaded");
    }
}