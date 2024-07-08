using Foster.Framework;
using Frosty.Scripts.Entities;
using Frosty.Scripts.StaticObjects;
using System.Numerics;

namespace Frosty.Scripts.Core;

public sealed class Application : Module
{

    public static float gravity = 20;

    public static Texture blockTexture { get; private set; }
    public static Aseprite playerIdle;

    public static Aseprite playerWalkRight;
    public static Aseprite playerWalkLeft;

    List<Block> blocks;
    Batcher batcher;
    Player player;
    Random random;

    public override void Startup()
    {
        blockTexture = new Texture(new Aseprite(Path.Combine("Assets", "Objects", "Block.ase")).Frames[0].Cels[0].Image);

        playerIdle = new Aseprite(Path.Combine("Assets", "Player", "player_idle.ase"));
        playerWalkRight = new Aseprite(Path.Combine("Assets", "Player", "player_walk_right.ase"));
        playerWalkLeft = new Aseprite(Path.Combine("Assets", "Player", "player_walk_left.ase"));

        blocks = new List<Block>();
        batcher = new Batcher();
        player = new Player(new Vector2(100, 100));
        random = new Random();


        for (int i = 0; i < 10; i++)
        {
            blocks.Add(new Block(new Vector2(i * 50, 300), new Vector2(3, 3)));
        }
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
