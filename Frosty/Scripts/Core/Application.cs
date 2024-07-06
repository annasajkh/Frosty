using Foster.Framework;
using Frosty.Scripts.Entities;
using Frosty.Scripts.StaticObjects;
using System.Numerics;

namespace Frosty.Scripts.Core;

public class Application : Module
{

    public static float gravity = 2000;

    public static Texture blockTexture { get; } = new Texture(new Image(Path.Combine("Assets", "StaticObjects", "Block.png")));

    List<Block> blocks = new();
    Batcher batcher = new();
    Player player = new(new Vector2(100, 100));
    Random random = new();

    public override void Startup()
    {
        for (int i = 0; i < 20; i++)
        {
            blocks.Add(new Block(new Vector2(random.Next() % App.Width, random.Next() % App.Height), new Vector2(3, 3)));
        }
    }

    public override void Update()
    {
        player.Update();

        foreach (var block in blocks)
        {
            block.ResolveAwayFrom(player);
        }
    }

    public override void Render()
    {
        Graphics.Clear(Color.CornflowerBlue);

        foreach (var block in blocks)
        {
            block.Draw(batcher);
            batcher.RectLine(block.Rect, 2, Color.Red);
        }

        player.Draw(batcher);
        batcher.RectLine(player.Rect, 2, Color.Red);

        batcher.Render();
        batcher.Clear();
    }

    public override void Shutdown()
    {

    }
}
