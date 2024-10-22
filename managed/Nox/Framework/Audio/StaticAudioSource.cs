using System;
using System.Linq;

namespace Nox.Framework.Audio;

public abstract class AbstractAudioPlayer : IAudioPlayer {
    private double _index = 0;

    public AbstractAudioPlayer(int sampleCount, int sampleRate = 44100, int channels = 2) {
        SampleCount = sampleCount;
        SampleRate = sampleRate;
        Channels = channels;
        Duration = sampleCount / (double)(sampleRate * channels);
    }

    public double Duration { get; private set; }
    public double Time => _index;
    public bool IsPlaying { get; private set; }

    public bool Loop { get; set; }
    public float Gain { get; set; } = 1;

    public int SampleRate { get; private set; }

    public int Channels { get; private set; }

    public int SampleCount { get; private set; }

    public StereoFrameF GetNextFrame(int sampleRate)
    {
        if(!IsPlaying) return StereoFrameF.Zero;
        if(_index >= Duration) {
            _index = _index%Duration;
            if(!Loop){
                IsPlaying = false;
                return StereoFrameF.Zero;
            }
        }
        var v = new StereoFrameF();
        GetSample(_index, out var l, out var r);
        v.L = l / 32768f * Gain;
        v.R = Channels == 2 ? r / 32768f * Gain : v.L;
        _index += 1.0 / sampleRate;
        return v;
    }

    protected abstract void GetSample(double index, out short l, out short r);

    public void Pause()
    {
        IsPlaying = false;
    }

    public void Play()
    {
        IsPlaying = true;
    }

    public void Seek(double time)
    {
        _index = Math.Min(time, Duration);
    }

    public void Stop()
    {
        IsPlaying = false;
        _index = 0;
    }
}

public class WavStreamingAudioPlayer : AbstractAudioPlayer
{
    public static WavStreamingAudioPlayer FromWavFile(string path){
        return new WavStreamingAudioPlayer(new WavFile(path));
    }

    public WavStreamingAudioPlayer(WavFile wav) : base(wav.SampleCount, wav.SampleRate, wav.Channels){
        Wav = wav;
    }

    public WavFile Wav { get; }

    protected override void GetSample(double index, out short l, out short r)
    {
        var offset = (int)(index * SampleRate * Channels);
        l = Wav.GetSample(offset);
        r = Channels == 2 ? Wav.GetSample(offset + 1) : l;
    }
}

public class StaticAudioPlayer : AbstractAudioPlayer {
    public static StaticAudioPlayer FromWavFile(string path){
        var wav = new WavFile(path);
        var frames = wav.EnumerateSamples().ToArray();
        return new StaticAudioPlayer(frames, wav.SampleRate, wav.Channels);
    }

    private readonly short[] _samples;
    
    public StaticAudioPlayer(short[] samples, int sampleRate = 44100, int channels = 2) 
        : base(samples.Length, sampleRate, channels) {
        _samples = samples;
    }

    protected override void GetSample(double index, out short l, out short r) {
        var offset = (int)(index * SampleRate * Channels);
        l = _samples[offset];
        r = Channels == 2 ? _samples[offset + 1] : l;
    }
}
