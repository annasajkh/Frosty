using Foster.Framework;

namespace Frosty.Scripts.Components;

public class TileMap
{
    public Texture Texture { get; }
    public string[] AsepritePath { get; }

    public int TileWidth { get; }
    public int TileHeight { get; }
    public int RowTotal { get; }
    public int ColumnTotal { get; }

    public TileMap(string[] asepritePath, int tileWidth, int tileHeight, int rowTotal, int columnTotal)
    {
        Texture = new Texture(new Aseprite(Path.Combine(asepritePath)).Frames[0].Cels[0].Image);
        AsepritePath = asepritePath;
        TileWidth = tileWidth;
        TileHeight = tileHeight;
        RowTotal = rowTotal;
        ColumnTotal = columnTotal;
    }

    public Rect GetRect(int index)
    { 
        int row = index / RowTotal;
        int column = index % RowTotal;

        return new Rect(
            x: column * TileWidth,
            y: row * TileHeight,
            w: TileWidth,
            h: TileHeight
        );
    }
}