using System.Text.Json.Serialization;

namespace Frosty.Scripts.Components;

[Serializable]
public struct LevelEditorSaveData
{
    public string[] asepritePath { get; set; }
    public string[] collectionAsepritePath { get; set; }
    public int tileWidth { get; set; }
    public int tileHeight { get; set; }
    public int totalRow { get; set; }
    public int totalColumn { get; set; }
    public List<Tile> tiles { get; set; }
    public List<Tile> tileCollection { get; set; }

    [JsonConstructor]
    public LevelEditorSaveData(string[] asepritePath, string[] collectionAsepritePath, int tileWidth, int tileHeight, int totalRow, int totalColumn, List<Tile> tiles, List<Tile> tileCollection)
    {
        this.asepritePath = asepritePath;
        this.collectionAsepritePath = collectionAsepritePath;
        this.tileWidth = tileWidth;
        this.tileHeight = tileHeight;
        this.totalRow = totalRow;
        this.totalColumn = totalColumn;
        this.tiles = tiles;
        this.tileCollection = tileCollection;
    }
}