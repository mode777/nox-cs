namespace Nox.Framework.Audio;

public interface IAudioSource {
    public float Gain { get; set; }
    StereoFrameF GetNextFrame(int sampleRate);
}
