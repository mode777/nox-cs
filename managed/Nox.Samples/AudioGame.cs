using System.Numerics;
using Microsoft.Xna.Framework;
using Nox.Framework;
using Nox.Framework.Audio;

namespace Nox;

public class AudioGame : Game {
    private SpriteBatch _batch;
    private SpriteFont _font;
    private AudioMixer _mixer;
    private IAudioPlayer _effect;
    private IAudioPlayer _music;

    public override void Init()
    {
        _batch = new SpriteBatch();

        _font = SpriteFont.Load("../../assets/open-sans.italic.ttf", 30);
        _effect = StaticAudioPlayer.FromWavFile("../../assets/sample1.WAV");

        _music = WavStreamingAudioPlayer.FromWavFile("../../assets/CantinaBand60.wav");
        _music.Play();
        _music.Loop = true;

        _mixer = new AudioMixer(2);
        _mixer[0] = _music;
        _mixer[1] = _effect;
        AudioDevice.AudioSource = _mixer; 
        Window.OnKeyPress += (ev) =>
        {
            if(ev.KeyCode == KeyCode.Space) _effect.Play();
        };
        base.Init();
    }

    public override void Update(double deltaTime)
    {
        _music.Gain = Mouse.Position.Y/GraphicsDevice.Size.Y;
        base.Update(deltaTime);
    }

    public override void Render()
    {
        _batch.Begin();
        _batch.DrawText(_font, TimeSpan.FromSeconds(_music.Time).ToString(@"hh\:mm\:ss") + "/" + TimeSpan.FromSeconds(_music.Duration).ToString(@"hh\:mm\:ss"), new Vector2(30,60), ColorRGBA.White);
        _batch.DrawText(_font, "Press space to play soundeffect", new Vector2(30,90), ColorRGBA.White);
        _batch.DrawText(_font, $"Music Volume: {(int)(_music.Gain*100)}%", new Vector2(30,120), ColorRGBA.White);
        _batch.End();
        base.Render();
    }
}