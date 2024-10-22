using static Nox.Native.LibNox;

namespace Nox.Framework.Audio;

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