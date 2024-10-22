namespace Nox.Framework.Audio;

public class NullAudioSource : IAudioSource
{
    public float Gain { get; set; }
    public StereoFrameF GetNextFrame(int sampleRate) => StereoFrameF.Zero;
}
