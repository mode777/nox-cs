namespace Nox.Framework.Audio;

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
