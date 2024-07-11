using Foster.Framework;
using Frosty.Scripts.Core;

namespace Frosty.Scripts;

internal static class Program
{
    static void Main(string[] args)
    {
        App.Register<Game>();
        App.Run("Frosty", 960, 528);
    }
}