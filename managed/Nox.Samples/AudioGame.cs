using System.Numerics;
using Microsoft.Xna.Framework;
using Nox.Framework;

namespace Nox;

public class AudioGame : Game {
    private SpriteBatch _batch;
    private SpriteFont _font;
    private AudioMixer _mixer;
    private StaticAudioSource _effect;
    private StaticAudioSource _music;

    public override void Init()
    {
        _batch = new SpriteBatch();

        _font = SpriteFont.Load("../../assets/open-sans.italic.ttf");
        _font.LoadGlyphs(30);
        _font.Update();
        
        _effect = StaticAudioSource.FromWavFile("../../assets/sample1.WAV");

        _music = StaticAudioSource.FromWavFile("../../assets/CantinaBand60.wav");
        _music.Play();
        _music.Loop = true;

        _mixer = new AudioMixer(2);
        _mixer[0] = _music;
        _mixer[1] = _effect;
        AudioDevice.AudioSource = _mixer; 
        base.Init();
    }

    public override void Update()
    {
        _music.Gain = Mouse.Position.Y/GraphicsDevice.Size.Y;
        base.Update();
    }

    public override void Render()
    {
        _batch.Begin();
        _batch.DrawText(_font, TimeSpan.FromSeconds(_music.Time).ToString(@"hh\:mm\:ss"), 30, new Vector2(30,60), ColorRGBA.White);
        _batch.DrawText(_font, "Press space to play soundeffect", 30, new Vector2(30,90), ColorRGBA.White);
        _batch.DrawText(_font, $"Music Volume: {(int)(_music.Gain*100)}%", 30, new Vector2(30,120), ColorRGBA.White);
        _batch.End();
        base.Render();
    }
}