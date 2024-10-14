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
    StereoFrameF GetNextFrame();
}

public interface IAudioPlayer {
    public double Time { get; }
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

    public StereoFrameF GetNextFrame()
    {
        var frame = StereoFrameF.Zero;
        foreach (var channel in _channels)
        {
            if(channel is not null){
                frame.Add(channel.GetNextFrame());
            }
        }
        frame.Gain(Gain);
        return frame;
    }
}

public class NullAudioSource : IAudioSource
{
    public float Gain { get; set; }
    public StereoFrameF GetNextFrame() => StereoFrameF.Zero;
}

public class StaticAudioSource : IAudioSource, IAudioPlayer {
    public static StaticAudioSource FromWavFile(string path){
        var wav = new WavFileReader(path);
        var frames = wav.ReadAllFrames();
        return new StaticAudioSource(frames);
    }

    private readonly StereoFrame16[] _frames;
    private int _index = 0;

    public StaticAudioSource(StereoFrame16[] samples) {
        _frames = samples;
    }

    public double Time => _index / 44100f;
    public bool IsPlaying { get; private set; }

    public bool Loop { get; set; }
    public float Gain { get; set; } = 1;

    public StereoFrameF GetNextFrame()
    {
        if(!IsPlaying) return StereoFrameF.Zero;
        if(_index >= _frames.Length) {
            _index = _index%_frames.Length;
            if(!Loop){
                IsPlaying = false;
                return StereoFrameF.Zero;
            }
        }
        var v = _frames[_index++].ToFloat();
        v.Gain(Gain); 
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
        _index = Math.Min((int)(time * 44100), _frames.Length-1);
    }

    public void Stop()
    {
        IsPlaying = false;
        _index = 0;
    }
}

public class AudioDevice {

    internal static IAudioSource AudioSource = new NullAudioSource();
    internal static unsafe void AudioCallback(float* buffer, int num_frames, int num_channels){
        if(num_channels == 2){
            for (int f = 0; f < num_frames; f++)
            {
                var frame = AudioSource.GetNextFrame();
                buffer[f*num_channels] = frame.L;
                buffer[f*num_channels+1] = frame.R;
            }
        } else {
            for (int f = 0; f < num_frames; f++)
            {
                var frame = AudioSource.GetNextFrame();
                buffer[f*num_channels] = (frame.L + frame.R) / 2f;
            }
        }
    }
} 