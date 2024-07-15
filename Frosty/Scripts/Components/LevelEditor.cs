using Foster.Framework;
using Frosty.Scripts.Core;
using Frosty.Scripts.DataStructures;
using Frosty.Scripts.StaticObjects;
using Frosty.Scripts.Utils;
using System.Numerics;
using Newtonsoft.Json;

namespace Frosty.Scripts.Components;

public enum EditingMode
{
    TileSet,
    TileCollection
}

public class LevelEditor
{
    public int CurrentTileIndex { get; private set; } = 0;

    public bool Editing { get; set; }

    public TileMap? TileMap { get; private set; }
    public TileCollection? TileCollection { get; private set; }

    public Dictionary<int, TileObject> Tiles { get; } = new();

    bool isDeleting;

    public EditingMode EditingMode { get; private set; }

    public LevelEditor(bool editing, EditingMode editingMode, TileMap tileMap, TileCollection tileCollection)
    {
        TileMap = tileMap;
        TileCollection = tileCollection;
        Editing = editing;
        EditingMode = editingMode;
    }

    public void Save(string path)
    {
        List<Tile> tilesToSave = new();
        List<Tile> tileCollectionToSave = new();

        foreach (var tile in Tiles)
        {
            if (tile.Value.tileType == TileType.Decoration)
            {
                tileCollectionToSave.Add(new Tile(tile.Value.position, tile.Value.TextureRect, tile.Value.tileType));
            }
            else
            {
                tilesToSave.Add(new Tile(tile.Value.position, tile.Value.TextureRect, tile.Value.tileType));
            }
        }

        LevelEditorSaveData levelEditorSaveData = new(TileMap.AsepritePath, TileCollection.AsepritePath, TileMap.TileWidth, TileMap.TileHeight, TileMap.RowTotal, TileMap.ColumnTotal, TileMap.TotalTiles, tilesToSave, tileCollectionToSave);

        string levelJson = JsonConvert.SerializeObject(levelEditorSaveData);

        string[] pathSplitted = path.Split(Path.DirectorySeparatorChar);
        string dirPath = Path.Join(pathSplitted.Take(pathSplitted.Length - 1).ToArray());

        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }

        File.WriteAllText(path, levelJson);
    }

    public void Load(string path)
    {
        var levelData = JsonConvert.DeserializeObject<LevelEditorSaveData>(File.ReadAllText(path));

        TileMap = new TileMap(levelData.asepritePath, levelData.tileWidth, levelData.tileHeight, levelData.totalRow, levelData.totalColumn, levelData.totalTiles);
        TileCollection = new TileCollection(levelData.collectionAsepritePath, TileCollection.Tiles);

        foreach (var tileData in levelData.tiles)
        {
            Tiles.Add(ToTileObject(tileData, tileData.tileType).GetHashCode(), ToTileObject(tileData, tileData.tileType));
        }

        foreach (var tileData in levelData.tileCollection)
        {
            Tiles.Add(ToTileObject(tileData, tileData.tileType).GetHashCode(), ToTileObject(tileData, tileData.tileType));
        }
    }

    public void Update()
     {
        if (!Editing)
        {
            return;
        }

        if (Input.Keyboard.Pressed(Keys.I))
        {
            EditingMode = EditingMode.TileSet;
            CurrentTileIndex = 0;
        }

        if (Input.Keyboard.Pressed(Keys.O))
        {
            EditingMode = EditingMode.TileCollection;
            CurrentTileIndex = 0;
        }

        switch (EditingMode)
        {
            case EditingMode.TileSet:
                if (Input.Mouse.Down(MouseButtons.Left))
                {
                    TileObject tileObject;

                    TileType tileType = TileType.Solid;

                    if (CurrentTileIndex == 9 || CurrentTileIndex == 10)
                    {
                        tileType = TileType.Spike;
                    }
                    else if (CurrentTileIndex == 11)
                    {
                        tileType = TileType.Ice;
                    }

                    tileObject = ToTileObject(new Tile(Helper.SnapToGrid(Input.Mouse.Position, (int)(Game.TileSize * Game.Scale)) + new Vector2(Game.TileSize * Game.Scale) / 2, TileMap.GetRect(CurrentTileIndex), tileType), tileType);

                    if (!Tiles.ContainsKey(tileObject.GetHashCode()))
                    {
                        Tiles.Add(tileObject.GetHashCode(), tileObject);
                    }
                }

                if (Input.Mouse.Down(MouseButtons.Right))
                {
                    isDeleting = true;

                    Vector2 mousePosSnapped = Helper.SnapToGrid(Input.Mouse.Position, (int)(Game.TileSize * Game.Scale)) + new Vector2(Game.TileSize * Game.Scale) / 2;

                    if (Tiles.ContainsKey(new Vector2i((int)mousePosSnapped.X, (int)mousePosSnapped.Y).GetHashCode()))
                    {
                        Tiles.Remove(new Vector2i((int)mousePosSnapped.X, (int)mousePosSnapped.Y).GetHashCode());
                    }
                }
                else
                {
                    isDeleting = false;
                }
                break;
            case EditingMode.TileCollection:

                if (Input.Mouse.Down(MouseButtons.Left))
                {
                    TileObject tileObject;
                    TileType tileType = TileType.Decoration;
                    Rect tileRect = new Rect(TileCollection.Tiles[CurrentTileIndex].x, TileCollection.Tiles[CurrentTileIndex].y, TileCollection.Tiles[CurrentTileIndex].rectWidth, TileCollection.Tiles[CurrentTileIndex].rectHeight);

                    tileObject = ToTileObject(new Tile(Helper.SnapToGrid(Input.Mouse.Position, (int)(Game.TileSize * Game.Scale)) + new Vector2(Game.TileSize * Game.Scale) / 2, tileRect, tileType), tileType);

                    if (!Tiles.ContainsKey(tileObject.GetHashCode()))
                    {
                        Tiles.Add(tileObject.GetHashCode(), tileObject);
                    }
                }

                if (Input.Mouse.Down(MouseButtons.Right))
                {
                    isDeleting = true;

                    Vector2 mousePosSnapped = Helper.SnapToGrid(Input.Mouse.Position, (int)(Game.TileSize * Game.Scale)) + new Vector2(Game.TileSize * Game.Scale) / 2;

                    if (Tiles.ContainsKey(new Vector2i((int)mousePosSnapped.X, (int)mousePosSnapped.Y).GetHashCode()))
                    {
                        Tiles.Remove(new Vector2i((int)mousePosSnapped.X, (int)mousePosSnapped.Y).GetHashCode());
                    }
                }
                else
                {
                    isDeleting = false;
                }
                break;
            default:
                break;
        }

        int mouseScroll = (int)Input.Mouse.Wheel.Y;

        if (mouseScroll > 0)
        {
            CurrentTileIndex += 1;
        }
        else if (mouseScroll < 0)
        {
            CurrentTileIndex -= 1;
        }

        switch (EditingMode)
        {
            case EditingMode.TileSet:
                if (CurrentTileIndex > TileMap.TotalTiles - 1)
                {
                    CurrentTileIndex = 0;
                }
                else if (CurrentTileIndex < 0)
                {
                    CurrentTileIndex = TileMap.TotalTiles - 1;
                }
                break;
            case EditingMode.TileCollection:
                if (CurrentTileIndex > TileCollection.Tiles.Count - 1)
                {
                    CurrentTileIndex = 0;
                }
                else if (CurrentTileIndex < 0)
                {
                    CurrentTileIndex = TileCollection.Tiles.Count - 1;
                }
                break;
            default:
                break;
        }
    }

    public TileObject? ToTileObject(Tile tile, TileType tileType)
    {
        if (tileType == TileType.Decoration)
        {
            return new TileObject(new Vector2(tile.x, tile.y), Vector2.One * Game.Scale, TileCollection.Texture, new Rect(tile.rectX, tile.rectY, tile.rectWidth, tile.rectHeight), tile.tileType);
        }
        else
        {
            return new TileObject(new Vector2(tile.x, tile.y), Vector2.One * Game.Scale, TileMap.Texture, new Rect(tile.rectX, tile.rectY, tile.rectWidth, tile.rectHeight), tile.tileType);
        }
    }

    public Tile ToTile(TileObject tileObject)
    {
        return new Tile(tileObject.position, tileObject.TextureRect, tileObject.tileType);
    }

    public void DrawWhenPaused(Batcher batcher)
    {
        batcher.PushMatrix(Vector2.Zero, Vector2.One * Game.Scale, Vector2.Zero, 0);

        switch (EditingMode)
        {
            case EditingMode.TileSet:
                batcher.Image(TileMap.Texture, Vector2.Zero, Color.White);
                batcher.RectLine(TileMap.GetRect(CurrentTileIndex), 1, Color.Red);
                break;
            case EditingMode.TileCollection:
                batcher.Image(TileCollection.Texture, Vector2.Zero, Color.White);

                Rect tileRect = new Rect(TileCollection.Tiles[CurrentTileIndex].x, TileCollection.Tiles[CurrentTileIndex].y, TileCollection.Tiles[CurrentTileIndex].rectWidth, TileCollection.Tiles[CurrentTileIndex].rectHeight);

                batcher.RectLine(tileRect, 1, Color.Red);
                break;
            default:
                break;
        }

        batcher.PopMatrix();
    }

    public void Draw(Batcher batcher)
    {
        if (!Editing)
        {
            return;
        }

        batcher.PushMatrix(Helper.SnapToGrid(Input.Mouse.Position, (int)(Game.TileSize * Game.Scale)) + new Vector2(Game.TileSize * Game.Scale) / 2, Vector2.One * Game.Scale, Vector2.One * Game.TileSize / 2, 0);

        switch (EditingMode)
        {
            case EditingMode.TileSet:
                if (isDeleting)
                {
                    batcher.Image(TileMap.Texture, TileMap.GetRect(CurrentTileIndex), Vector2.Zero, new Color(100, 0, 0, 100));
                }
                else
                {
                    batcher.Image(TileMap.Texture, TileMap.GetRect(CurrentTileIndex), Vector2.Zero, new Color(100, 100, 100, 100));
                }
                break;
            case EditingMode.TileCollection:
                Rect tileRect = new Rect(TileCollection.Tiles[CurrentTileIndex].x, TileCollection.Tiles[CurrentTileIndex].y, TileCollection.Tiles[CurrentTileIndex].rectWidth, TileCollection.Tiles[CurrentTileIndex].rectHeight);

                if (isDeleting)
                {
                    batcher.Image(TileCollection.Texture, tileRect, Vector2.Zero, new Color(100, 0, 0, 100));
                }
                else
                {
                    batcher.Image(TileCollection.Texture, tileRect, Vector2.Zero, new Color(100, 100, 100, 100));
                }
                break;
            default:
                break;
        }

        batcher.PopMatrix();
    }
}