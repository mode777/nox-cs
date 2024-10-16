using Nox;
using Nox.Framework;

var options = new ApplicationConfiguration {
    Width = 1280,
    Height = 720,
    Fullscreen = true,
};

Application.Run<MyGame>(options);

//var game = new AudioGame();
//var game = new ShaderGame();
//var game = new SpriteFontGame();
//var game = new SpriteAtlasGame();
//var game = new MyGame();
