using Foster.Framework;
using Frosty.Scripts.Core;
using Frosty.Scripts.DataStructures;
using Frosty.Scripts.StaticObjects;
using Frosty.Scripts.Utils;
using System.Numerics;

namespace Frosty.Scripts.Components;

public class LevelEditor
{
    public int CurrentTileIndex { get; private set; } = 0;
    public int TotalTiles { get; }

    public bool Editing { get; set; }
    
    public Tileset Tileset { get; }
    public Dictionary<int, TileObject> Tiles { get; } = new();

    bool isDeleting = false;

    public LevelEditor(Tileset tileset, bool editing, int totalTiles)
    {
        Tileset = tileset;
        Editing = editing;
        TotalTiles = totalTiles;
    }

    public void Update()
    {
        if (!Editing)
        {
            return;
        }

        if (Input.Mouse.Down(MouseButtons.Left))
        {
            TileObject tileOject = ToTileObject(new Tile(Helper.SnapToGrid(Input.Mouse.Position, (int)(Game.TileSize * Game.Scale)) + new Vector2(Game.TileSize * Game.Scale) / 2, Tileset.GetRect(CurrentTileIndex)));

            if (!Tiles.ContainsKey(tileOject.GetHashCode()))
            {
                Tiles.Add(tileOject.GetHashCode() ,tileOject);
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

        if (CurrentTileIndex > TotalTiles - 1)
        {
            CurrentTileIndex = 0;
        }
        else if (CurrentTileIndex < 0)
        {
            CurrentTileIndex = TotalTiles - 1;
        }
    }

    public TileObject ToTileObject(Tile tile)
    {
        return new TileObject(tile.Position, Vector2.One * Game.Scale, Tileset.Texture, tile.Rect);
    }

    public Tile ToTile(TileObject tileObject)
    {
        return new Tile(tileObject.position, tileObject.Rect);
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
