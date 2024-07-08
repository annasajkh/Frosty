using Foster.Framework;
using Frosty.Scripts.Core;

namespace Frosty.Scripts;

internal class Program
{
    static void Main(string[] args)
    {
        App.Register<Application>();
        App.Run("Frosty", 960, 540);
    }
}