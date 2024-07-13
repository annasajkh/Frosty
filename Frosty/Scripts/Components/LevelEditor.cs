using Foster.Framework;
using Frosty.Scripts.Core;
using Frosty.Scripts.DataStructures;
using Frosty.Scripts.StaticObjects;
using Frosty.Scripts.Utils;
using System.Numerics;
using Newtonsoft.Json;

namespace Frosty.Scripts.Components;

public class LevelEditor
{
    public int CurrentTileIndex { get; private set; } = 0;

    public bool Editing { get; set; }

    public Tileset? Tileset { get; private set; }
    public Dictionary<int, TileObject> Tiles { get; } = new();

    bool isDeleting;

    public LevelEditor(bool editing, Tileset? tileset = null)
    {
        Tileset = tileset;
        Editing = editing;
    }

    public void Save(string path)
    {
        List<Tile> tilesToSave = new();

        foreach (var tile in Tiles)
        {
            tilesToSave.Add(new Tile(tile.Value.position, tile.Value.TextureRect, tile.Value.tileType));
        }

        LevelEditorSaveData levelEditorSaveData = new(Tileset.AsepritePath, Tileset.TileWidth, Tileset.TileHeight, Tileset.RowTotal, Tileset.ColumnTotal, Tileset.TotalTiles, tilesToSave);

        string levelJson = JsonConvert.SerializeObject(levelEditorSaveData);

        File.WriteAllText(path, levelJson);
    }

    public void Load(string path)
    {
        var levelData = JsonConvert.DeserializeObject<LevelEditorSaveData>(File.ReadAllText(path));

        Tileset = new Tileset(levelData.asepritePath, levelData.tileWidth, levelData.tileHeight, levelData.totalRow, levelData.totalColumn, levelData.totalTiles);

        foreach (var tileData in levelData.tiles)
        {
            Tiles.Add(ToTileObject(tileData).GetHashCode(), ToTileObject(tileData));
        }
    }

    public void Update()
    {
        if (!Editing)
        {
            return;
        }

        if (Input.Mouse.Down(MouseButtons.Left))
        {
            TileObject tileObject;

            TileType tileType = TileType.Solid;

            if (CurrentTileIndex == 9 || CurrentTileIndex == 10)
            {
                tileType = TileType.Spike;
            }
            else if(CurrentTileIndex == 11)
            {
                tileType = TileType.Ice;
            }

            tileObject = ToTileObject(new Tile(Helper.SnapToGrid(Input.Mouse.Position, (int)(Game.TileSize * Game.Scale)) + new Vector2(Game.TileSize * Game.Scale) / 2, Tileset.GetRect(CurrentTileIndex), tileType));


            if (!Tiles.ContainsKey(tileObject.GetHashCode()))
            {
                Tiles.Add(tileObject.GetHashCode() ,tileObject);
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

        int mouseScroll = (int)Input.Mouse.Wheel.Y;

        if (mouseScroll > 0)
        {
            CurrentTileIndex += 1;
        }
        else if (mouseScroll < 0)
        {
            CurrentTileIndex -= 1;
        }

        if (CurrentTileIndex > Tileset.TotalTiles - 1)
        {
            CurrentTileIndex = 0;
        }
        else if (CurrentTileIndex < 0)
        {
            CurrentTileIndex = Tileset.TotalTiles - 1;
        }
    }

    public TileObject ToTileObject(Tile tile)
    {
        return new TileObject(new Vector2(tile.x, tile.y), Vector2.One * Game.Scale, Tileset.Texture, new Rect(tile.rectX, tile.rectY, tile.rectWidth, tile.rectHeight), tile.tileType);
    }

    public Tile ToTile(TileObject tileObject)
    {
        return new Tile(tileObject.position, tileObject.TextureRect, tileObject.tileType);
    }

    public void DrawWhenPaused(Batcher batcher)
    {
        batcher.PushMatrix(Vector2.Zero, Vector2.One * Game.Scale, Vector2.Zero, 0);

        batcher.Image(Tileset.Texture, Vector2.Zero, Color.White);
        batcher.RectLine(Tileset.GetRect(CurrentTileIndex), 1, Color.Red);

        batcher.PopMatrix();
    }

    public void Draw(Batcher batcher)
    {
        if (!Editing)
        {
            return;
        }

        batcher.PushMatrix(Helper.SnapToGrid(Input.Mouse.Position, (int)(Game.TileSize * Game.Scale)) + new Vector2(Game.TileSize * Game.Scale) / 2, Vector2.One * Game.Scale, Vector2.One * Game.TileSize / 2, 0);

        if (isDeleting)
        {
            batcher.Rect(Vector2.Zero, Vector2.One * Game.TileSize, new Color(100, 0, 0, 100));
        }
        else
        {
            batcher.Image(Tileset.Texture, Tileset.GetRect(CurrentTileIndex), Vector2.Zero, new Color(100, 100, 100, 100));
        }

        batcher.PopMatrix();
    }
}