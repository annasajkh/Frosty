namespace Frosty.Scripts.DataStructures;

[Serializable]
public struct LevelEditorSaveData
{
    public string[] asepritePath;

    public int tileWidth;
    public int tileHeight;
    public int totalRow;
    public int totalColumn;
    public int totalTiles;
    public List<Tile> tiles;

    public LevelEditorSaveData(string[] asepritePath, int tileWidth, int tileHeight, int rowTotal, int columnTotal, int totalTiles, List<Tile> tiles)
    {
        this.asepritePath = asepritePath;
        this.tileWidth = tileWidth;
        this.tileHeight = tileHeight;
        this.totalRow = rowTotal;
        this.totalColumn = columnTotal;
        this.totalTiles = totalTiles;
        this.tiles = tiles;
    }
}
