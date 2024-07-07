using Foster.Framework;
using Frosty.Scripts.Entities;
using Frosty.Scripts.StaticObjects;
using System.Numerics;

namespace Frosty.Scripts.Core;

public class Application : Module
{

    public static float gravity = 20;

    public static Texture blockTexture { get; } = new Texture(new Image(Path.Combine("Assets", "StaticObjects", "Block.png")));

    List<Block> blocks = new();
    Batcher batcher = new();
    Player player = new(new Vector2(100, 100));
    Random random = new();

    public override void Startup()
    {

        blocks.Add(new Block(new Vector2(50, 250), new Vector2(3, 3)));
        blocks.Add(new Block(new Vector2(200, 300), new Vector2(3, 3)));
        blocks.Add(new Block(new Vector2(300, 400), new Vector2(3, 3)));
    }

    public override void Update()
    {
        player.Update();

        foreach (var block in blocks)
        {
            player.ResolveAwayFrom(block);
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
