using Foster.Framework;
using Frosty.Scripts.Utils;
using System.Numerics;
using Newtonsoft.Json;
using Frosty.Scripts.GameObjects.StaticObjects;
using Frosty.Scripts.Core;
using Frosty.Scripts.Scenes.Levels;
using Frosty.Scripts.GameObjects.StaticTiles;

namespace Frosty.Scripts.Components;

public enum EditingMode
{
    TileSet,
    TileCollection
}

public class LevelEditor
{
    public Level Level { get; }
    public int CurrentTileIndex { get; private set; } = 0;
    public bool Editing { get; set; }

    public TileMap? TileMap { get; private set; }
    public TileCollection? TileCollection { get; private set; }

    public Dictionary<int, TileObject> Tiles { get; } = new();

    bool isDeleting;

    public EditingMode EditingMode { get; private set; }

    public LevelEditor(bool editing, EditingMode editingMode, TileMap tileMap, TileCollection tileCollection, Level level)
    {
        Level = level;
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
            if (tile.Value is Decoration)
            {
                tileCollectionToSave.Add(ToTile(tile.Value));
            }
            else
            {
                tilesToSave.Add(ToTile(tile.Value));
            }
        }

        LevelEditorSaveData levelEditorSaveData = new(TileMap.AsepritePath, TileCollection.AsepritePath, TileMap.TileWidth, TileMap.TileHeight, TileMap.RowTotal, TileMap.ColumnTotal, tilesToSave, tileCollectionToSave);

        string levelJson = JsonConvert.SerializeObject(levelEditorSaveData, Formatting.Indented);

        string[] pathSplitted = path.Split(Path.DirectorySeparatorChar);
        string dirPath = Path.Join(pathSplitted.Take(pathSplitted.Length - 1).ToArray());

        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }

        try
        {
            File.WriteAllText(path, levelJson);
        }
        catch(Exception exception)
        {
            Log.Warning($"Cannot auto save the level {exception}");
        }
    }

    public void Load(string path)
    {
        var levelData = JsonConvert.DeserializeObject<LevelEditorSaveData>(File.ReadAllText(path));

        TileMap = new TileMap(levelData.asepritePath, levelData.tileWidth, levelData.tileHeight, levelData.totalRow, levelData.totalColumn);
        TileCollection = new TileCollection(levelData.collectionAsepritePath, TileCollection.Tiles);

        foreach (var tileData in levelData.tiles)
        {
            Tiles.Add(new Vector2i(tileData.x, tileData.y).GetHashCode(), ToTileObject(tileData, tileData.tileType));
        }

        foreach (var tileData in levelData.tileCollection)
        {
            Tiles.Add(new Vector2i(tileData.x, tileData.y).GetHashCode(), ToTileObject(tileData, tileData.tileType));
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
                if (Input.Mouse.Down(MouseButtons.Left) && !Level.Paused)
                {
                    TileObject tileObject;

                    TileType tileType = TileType.Solid;

                    if (CurrentTileIndex == 9 || CurrentTileIndex == 10 || CurrentTileIndex == 12 || CurrentTileIndex == 13)
                    {
                        tileType = TileType.Spike;
                    }
                    else if (CurrentTileIndex == 11)
                    {
                        tileType = TileType.Ice;
                    }
                    else if (CurrentTileIndex == 39)
                    {
                        tileType = TileType.BrittleIce;
                    }

                    tileObject = ToTileObject(new Tile(Helper.SnapToGrid(Input.Mouse.Position, (int)(Game.TileSize * Game.Scale)) + new Vector2(Game.TileSize * Game.Scale) / 2, TileMap.GetRect(CurrentTileIndex), tileType), tileType);

                    if (!Tiles.ContainsKey(tileObject.GetHashCode()))
                    {
                        Tiles.Add(tileObject.GetHashCode(), tileObject);
                    }
                    else
                    {
                        Tiles.Remove(tileObject.GetHashCode());
                        Tiles.Add(tileObject.GetHashCode(), tileObject);
                    }
                }

                if (Input.Mouse.Down(MouseButtons.Right) && !Level.Paused)
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

        if (EditingMode == EditingMode.TileSet)
        {
            for (int i = 0; i < TileMap.RowTotal * TileMap.ColumnTotal; i++)
            {
                Rect tileMapRect = TileMap.GetRect(i) * 3;

                if (tileMapRect.Contains(Input.Mouse.Position) && Input.Mouse.Pressed(MouseButtons.Left) && Level.Paused)
                {
                    CurrentTileIndex = i;
                }
            }
        }
        else if (EditingMode == EditingMode.TileCollection)
        {
            for (int i = 0; i < TileCollection.Tiles.Count; i++)
            {
                Rect tileCollectionRect = new Rect(TileCollection.Tiles[i].rectX * Game.Scale, TileCollection.Tiles[i].rectY * Game.Scale, TileCollection.Tiles[i].rectWidth * Game.Scale, TileCollection.Tiles[i].rectHeight * Game.Scale);

                if (tileCollectionRect.Contains(Input.Mouse.Position) && Input.Mouse.Pressed(MouseButtons.Left) && Level.Paused)
                {
                    CurrentTileIndex = i;
                }
            }
        }
    }

    public TileObject? ToTileObject(Tile tile, TileType tileType)
    {
        if (tileType == TileType.Decoration)
        {
            return Helper.CreateTileObjectUsingTileType(new Vector2(tile.x, tile.y), Vector2.One * Game.Scale, TileCollection.Texture, new Rect(tile.rectX, tile.rectY, tile.rectWidth, tile.rectHeight), tile.tileType);
        }
        else
        {
            return Helper.CreateTileObjectUsingTileType(new Vector2(tile.x, tile.y), Vector2.One * Game.Scale, TileMap.Texture, new Rect(tile.rectX, tile.rectY, tile.rectWidth, tile.rectHeight), tile.tileType);
        }
    }

    public Tile ToTile(TileObject tileObject)
    {
        return new Tile(tileObject.position, tileObject.TextureRect, Helper.TileObjectToTileType(tileObject));
    }

    public void DrawWhenPaused(Batcher batcher)
    {
        batcher.PushMatrix(Vector2.Zero, Vector2.One * Game.Scale, Vector2.Zero, 0);
        switch (EditingMode)
        {
            case EditingMode.TileSet:
                batcher.Image(TileMap.Texture, Vector2.Zero, Color.White);
                break;
            case EditingMode.TileCollection:
                batcher.Image(TileCollection.Texture, Vector2.Zero, Color.White);
                break;
            default:
                break;
        }
        batcher.PopMatrix();

        
        if (EditingMode == EditingMode.TileSet)
        {
            batcher.PushMatrix(Vector2.Zero, Vector2.One, Vector2.Zero, 0);

            for (int i = 0; i < TileMap.RowTotal * TileMap.ColumnTotal; i++)
            {
                Rect tileMapRect = TileMap.GetRect(i) * 3;
                batcher.RectLine(tileMapRect, 1, Color.Red);
            }

            batcher.PopMatrix();
        }
        else if (EditingMode == EditingMode.TileCollection)
        {
            batcher.PushMatrix(Vector2.Zero, Vector2.One * Game.Scale, Vector2.Zero, 0);

            for (int i = 0; i < TileCollection.Tiles.Count; i++)
            {
                Rect tileCollectionRect = new Rect(TileCollection.Tiles[i].rectX, TileCollection.Tiles[i].rectY, TileCollection.Tiles[i].rectWidth, TileCollection.Tiles[i].rectHeight);
                batcher.RectLine(tileCollectionRect, 1, Color.Red);
            }

            batcher.PopMatrix();
        }

        batcher.PushMatrix(Vector2.Zero, Vector2.One * Game.Scale, Vector2.Zero, 0);
        switch (EditingMode)
        {
            case EditingMode.TileSet:
                batcher.RectLine(TileMap.GetRect(CurrentTileIndex), 1, Color.Green);
                break;
            case EditingMode.TileCollection:
                Rect tileRect = new Rect(TileCollection.Tiles[CurrentTileIndex].x, TileCollection.Tiles[CurrentTileIndex].y, TileCollection.Tiles[CurrentTileIndex].rectWidth, TileCollection.Tiles[CurrentTileIndex].rectHeight);

                batcher.RectLine(tileRect, 1, Color.Green);
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

        if (!Level.Paused)
        {
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
        }

        batcher.PopMatrix();
    }
}