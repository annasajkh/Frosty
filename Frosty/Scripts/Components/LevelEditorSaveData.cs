namespace Frosty.Scripts.Components;

[Serializable]
public struct LevelEditorSaveData
{
    public string[] asepritePath;
    public string[] collectionAsepritePath;

    public int tileWidth;
    public int tileHeight;
    public int totalRow;
    public int totalColumn;

    public List<Tile> tiles;
    public List<Tile> tileCollection;

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
