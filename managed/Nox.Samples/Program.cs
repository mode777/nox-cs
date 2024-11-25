using Nox;
using Nox.Framework;
using Nox.Samples;
using Nox.Samples.CityBuilder;

var options = new ApplicationConfiguration {
    Width = 1280,
    Height = 720,
    Fullscreen = false,
    HighDpi = false
};

//Application.Run<FlowerTycoonGame>(options);
Application.Run<GlobalTransformGame>(options);

//var game = new AudioGame();
//var game = new ShaderGame();
//var game = new SpriteFontGame();
//var game = new SpriteAtlasGame();
//var game = new MyGame();
