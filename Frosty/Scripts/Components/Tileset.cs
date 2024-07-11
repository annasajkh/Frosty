using Foster.Framework;

namespace Frosty.Scripts.Components;

public class Tileset
{
    public Texture Texture { get; }

    public int TileWidth { get; }
    public int TileHeight { get; }
    public int RowTotal { get; }
    public int ColumnTotal { get; }

    public Tileset(Texture texture, int tileWidth, int tileHeight, int rowTotal, int columnTotal)
    {
        Texture = texture;
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