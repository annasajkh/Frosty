namespace Frosty.Scripts.DataStructures;

[Serializable]
public struct LevelEditorSaveData
{
    public string[] asepritePath;
    public string[] collectionAsepritePath;

    public int tileWidth;
    public int tileHeight;
    public int totalRow;
    public int totalColumn;
    public int totalTiles;

    public List<Tile> tiles;
    public List<Tile> tileCollection;

    public LevelEditorSaveData(string[] asepritePath, string[] collectionAsepritePath, int tileWidth, int tileHeight, int rowTotal, int columnTotal, int totalTiles, List<Tile> tiles, List<Tile> tileCollection)
    {
        this.asepritePath = asepritePath;
        this.collectionAsepritePath = collectionAsepritePath;
        this.tileWidth = tileWidth;
        this.tileHeight = tileHeight;
        this.totalRow = rowTotal;
        this.totalColumn = columnTotal;
        this.totalTiles = totalTiles;
        this.tiles = tiles;
        this.tileCollection = tileCollection;
    }
}
