using System.Text.Json.Serialization;

namespace Frosty.Scripts.Components;

[Serializable]
public struct SaveData
{
    public int playerDeath;
    public double gameRunTime;
    public string level;

    [JsonConstructor]
    public SaveData(int playerDeath, double gameRunTime, string level)
    {
        this.playerDeath = playerDeath;
        this.gameRunTime = gameRunTime;
        this.level = level;
    }
}
