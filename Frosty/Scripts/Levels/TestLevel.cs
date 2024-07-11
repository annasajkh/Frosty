using Foster.Framework;

namespace Frosty.Scripts.Levels;

public class TestLevel : Level
{
    public override void Startup()
    {
        base.Startup();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Render(Batcher batcher)
    {
        base.Render(batcher);
    }

    public override void Shutdown()
    {
        base.Update();
    }
}
