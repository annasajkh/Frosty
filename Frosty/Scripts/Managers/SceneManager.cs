using Frosty.Scripts.Abstracts;

namespace Frosty.Scripts.Managers;

public sealed class SceneManager
{
    /// <summary>
    /// The active scene that is updating and drawing to the screen
    /// </summary>
    public Scene? ActiveScene { get; private set; }

    private Dictionary<string, Scene> scenes = new Dictionary<string, Scene>();

    public SceneManager(string initialSceneName, Scene initialScene)
    {
        scenes.Add(initialSceneName, initialScene);
        ActiveScene = scenes[initialSceneName];
        ActiveScene.StartupInternal();
    }

    /// <summary>
    /// Add a scene to the scene manager
    /// </summary>
    /// <param name="name">The name of the scene</param>
    /// <param name="scene">The scene</param>
    public void AddScene(string name, Scene scene)
    {
        scenes.Add(name, scene);
    }

    /// <summary>
    /// Remove a scene from the scene manager
    /// </summary>
    /// <param name="name">The scene name</param>
    /// <exception cref="Exception">Exception if the the scene manager doesn't have an active scene or if you trying to remove an active scene</exception>
    public void RemoveScene(string name)
    {
        if (ActiveScene == null)
        {
            throw new Exception("Error: Scene manager doesn't contain active scene");
        }

        if (ActiveScene == scenes[name])
        {
            throw new Exception("Error: Cannot unload active scene");
        }

        ActiveScene.ShutdownInternal();
        scenes.Remove(name);
    }

    /// <summary>
    /// Change to different scene
    /// </summary>
    /// <param name="name">The name of the scene</param>
    /// <exception cref="Exception">Exception if the scene manager doesn't have an active scene</exception>
    public void ChangeScene(string name)
    {
        if (ActiveScene == null)
        {
            throw new Exception("Error: Scene Manager doesn't contain active scene");
        }

        ActiveScene.ShutdownInternal();
        ActiveScene = scenes[name];

        ActiveScene.StartupInternal();
    }
}
