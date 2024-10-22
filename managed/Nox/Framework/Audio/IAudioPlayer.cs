namespace Nox.Framework.Audio;

public interface IAudioPlayer : IAudioSource {
    public double Time { get; }
    public double Duration { get; }
    public bool IsPlaying { get; }
    public bool Loop { get; set; }
    public void Play();
    public void Pause();
    public void Stop();
    public void Seek(double time);
}
