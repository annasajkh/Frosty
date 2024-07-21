using Foster.Framework;
using Frosty.Scripts.Core;

namespace Frosty.Scripts;

internal static class Program
{
    static void Main(string[] args)
    {
#if DEBUG
        App.Resizable = true;
#else
        App.Resizable = false;
#endif
        App.Register<Game>();
        App.Run(applicationName: "Frosty", width: 960, height: 528);
    }
}