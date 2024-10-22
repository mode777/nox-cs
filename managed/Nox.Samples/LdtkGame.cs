using ldtk;
using Nox.Framework;

namespace Nox.Samples;

public class LdtkGame : Game {
    public override void Init()
    {
        var json = File.ReadAllText("../../assets/top_down.ldtk");
        var ldtk = LdtkJson.FromJson(json);
        base.Init();
    }
}