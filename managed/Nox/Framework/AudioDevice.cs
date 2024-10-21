using System;
using System.Linq;
using static Nox.Native.LibNox;

namespace Nox.Framework;

public struct StereoFrameF {
    public static readonly StereoFrameF Zero = new StereoFrameF { L = 0, R = 0 };
    public float L;
    public float R;

    public void Add(StereoFrameF frame){
        L += frame.L;
        R += frame.R;
    }

    public void Gain(float l, float r){
        L *= l;
        R *= r;
    }

    public void Gain(float c){
        L *= c;
        R *= c;
    }
}

public struct StereoFrame16 {
    public short L;
    public short R;

    public StereoFrameF ToFloat(){
        return new StereoFrameF() {
            L = L / 32768f,
            R = R / 32768f
        };
    }
}

public interface IAudioSource {
    public float Gain { get; set; }
    StereoFrameF GetNextFrame(int sampleRate);
}

public interface IAudioPlayer {
    public double Time { get; }
    public double Duration { get; }
    public bool IsPlaying { get; }
    public bool Loop { get; set; }
    public void Play();
    public void Pause();
    public void Stop();
    public void Seek(double time);
}

public class AudioMixer : IAudioSource
{
    private IAudioSource[] _channels;

    public AudioMixer(int channels)
    {
        Channels = channels;
        _channels = new IAudioSource[channels];
    }

    public int Channels { get; }
    public float Gain { get; set; } = 1;

    public IAudioSource this[int i]
   {
      get => _channels[i];
      set => _channels[i] = value;
   }

    public StereoFrameF GetNextFrame(int sampleRate)
    {
        var frame = StereoFrameF.Zero;
        foreach (var channel in _channels)
        {
            if(channel is not null){
                frame.Add(channel.GetNextFrame(sampleRate));
            }
        }
        frame.Gain(Gain);
        return frame;
    }
}

public class NullAudioSource : IAudioSource
{
    public float Gain { get; set; }
    public StereoFrameF GetNextFrame(int sampleRate) => StereoFrameF.Zero;
}

public class StaticAudioSource : IAudioSource, IAudioPlayer {
    public static StaticAudioSource FromWavFile(string path){
        var wav = new WavFile(path);
        var frames = wav.EnumerateSamples().ToArray();
        return new StaticAudioSource(frames, wav.SampleRate, wav.Channels);
    }

    private readonly short[] _samples;
    private readonly int _sampleRate;
    private readonly int _channels;
    private double _index = 0;

    public StaticAudioSource(short[] samples, int sampleRate = 44100, int channels = 2) {
        _samples = samples;
        _sampleRate = sampleRate;
        _channels = channels;
    }

    public double Duration => _samples.Length / (double)(_sampleRate * _channels);
    public double Time => _index;
    public bool IsPlaying { get; private set; }

    public bool Loop { get; set; }
    public float Gain { get; set; } = 1;

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
        var offset = (int)(_index * _sampleRate *_channels);
        v.L = _samples[offset] / 32768f * Gain;
        v.R = _channels == 2 ? _samples[offset + 1] / 32768f * Gain : v.L;
        _index += 1.0 / sampleRate;
        return v;
    }

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

public class AudioDevice {

    public static IAudioSource AudioSource = new NullAudioSource();
    public static int SampleRate {
        get {
            nox_sample_rate(out var rate);
            return rate;
        }
    }
    internal static unsafe void AudioCallback(float* buffer, int num_frames, int num_channels){
        var sampleRate = SampleRate;
        if(num_channels == 2){
            for (int f = 0; f < num_frames; f++)
            {
                var frame = AudioSource.GetNextFrame(sampleRate);
                buffer[f*num_channels] = frame.L;
                buffer[f*num_channels+1] = frame.R;
            }
        } else {
            for (int f = 0; f < num_frames; f++)
            {
                var frame = AudioSource.GetNextFrame(sampleRate);
                buffer[f*num_channels] = (frame.L + frame.R) / 2f;
            }
        }
    }
} 