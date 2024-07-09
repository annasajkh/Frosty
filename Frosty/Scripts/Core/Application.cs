using Foster.Framework;
using Frosty.Scripts.Components;
using Frosty.Scripts.Entities;
using Frosty.Scripts.StaticObjects;
using System.Numerics;

namespace Frosty.Scripts.Core;

public sealed class Application : Module
{

    public static float gravity = 20;

    public static Texture blockTexture { get; private set; }

    public static Aseprite playerIdleLeft;
    public static Aseprite playerIdleRight;

    public static Aseprite playerWalkRight;
    public static Aseprite playerWalkLeft;

    public static Random Random { get; } = new(Time.Now.Milliseconds);

    List<Block> blocks;
    Batcher batcher;
    Player player;
    Random random;

    Snowing snowing;

    public override void Startup()
    {
        snowing = new Snowing(new Vector2(0, 0), 0.005f, App.Width);

        blockTexture = new Texture(new Aseprite(Path.Combine("Assets", "Objects", "Block.ase")).Frames[0].Cels[0].Image);

        playerIdleLeft = new Aseprite(Path.Combine("Assets", "Player", "player_idle_left.ase"));
        playerIdleRight = new Aseprite(Path.Combine("Assets", "Player", "player_idle_right.ase"));

        playerWalkRight = new Aseprite(Path.Combine("Assets", "Player", "player_walk_right.ase"));
        playerWalkLeft = new Aseprite(Path.Combine("Assets", "Player", "player_walk_left.ase"));

        blocks = new List<Block>();
        batcher = new Batcher();
        player = new Player(new Vector2(100, 100));
        random = new Random();


        for (int i = 0; i < 10; i++)
        {
            if (i % 2 == 0)
            {
                blocks.Add(new Block(new Vector2(100 + i * 100, 300), new Vector2(3, 3)));
            }
        }
    }

    public override void Update()
    {
        player.Update();

        foreach (var block in blocks)
        {
            player.ResolveAwayFrom(block);
        }

        snowing.Update();
    }

    public override void Render()
    {
        Graphics.Clear(Color.CornflowerBlue);

        foreach (var block in blocks)
        {
            block.Draw(batcher);
        }

        player.Draw(batcher);

        snowing.Draw(batcher);

        batcher.Render();
        batcher.Clear();
    }

    public override void Shutdown()
    {

    }
}
