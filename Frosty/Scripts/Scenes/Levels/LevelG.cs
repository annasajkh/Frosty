﻿using System.Numerics;

namespace Frosty.Scripts.Scenes.Levels;

public class LevelG : Level
{
    public override void Startup()
    {
        base.Startup();

        player.position = new Vector2(30, 450);

        playerAtFinishLine += () =>
        {
            GoToNextLevel("LevelH");
        };
    }
}
