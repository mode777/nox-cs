using System;
using System.Drawing;
using System.IO;

namespace Nox.Framework;

public class WavFileReader
{
    private readonly int _startOffset;
    private readonly Stream _stream;
    private readonly BinaryReader _reader;
    public int SampleRate { get; private set; }
    public int BitsPerSample { get; private set; }
    public int Channels { get; private set; }
    public double Duration { get; }
    public int DataSize { get; }
    public int ByteRate { get; }
    public int Frames { get; }
    
    public WavFileReader(string filePath)
    {
        _stream = File.OpenRead(filePath);
        _reader = new BinaryReader(_stream);
        // Read RIFF header
        _reader.ReadChars(4); // "RIFF"
        _reader.ReadInt32(); // Chunk size
        string format = new string(_reader.ReadChars(4)); // "WAVE"
        if(format != "WAVE") throw new InvalidDataException("Not a wav file");
        // Read fmt subchunk
        _reader.ReadChars(4); // "fmt "
        int subchunk1Size = _reader.ReadInt32();
        short audioFormat = _reader.ReadInt16(); // PCM = 1
        if(audioFormat != 1) throw new InvalidDataException("Only PCM wav files supported"); 
        Channels = _reader.ReadInt16(); // Mono = 1, Stereo = 2
        System.Console.WriteLine("Channels:" + Channels);
        SampleRate = _reader.ReadInt32();
        System.Console.WriteLine("SampleRate:" + SampleRate);
        ByteRate = _reader.ReadInt32();
        short blockAlign = _reader.ReadInt16();
        BitsPerSample = _reader.ReadInt16();
        System.Console.WriteLine("Bits per sample: " + BitsPerSample);

        // Skip any extra bytes in the fmt subchunk (not needed for PCM)
        if (subchunk1Size > 16)
            _reader.ReadBytes(subchunk1Size - 16);

        // Read data subchunk
        _reader.ReadChars(4); // "data"
        DataSize = _reader.ReadInt32();
        Duration = (double)DataSize / ByteRate;
        Frames = DataSize / ((BitsPerSample / 8) * Channels);
        _startOffset = (int)_stream.Position;
    }

    // public void Seek(double seconds){
    //     if(seconds >= Duration) return;
    //     var bytes = (int)(seconds * ByteRate);
    //     _stream.Seek(_startOffset + bytes, SeekOrigin.Begin);
    // }

    public StereoFrame16[] ReadAllFrames() {
        if(BitsPerSample != 16) throw new InvalidOperationException("Cannot read 16-bit Stereo samples, need conversion");
        _stream.Seek(_startOffset, SeekOrigin.Begin);
        if(SampleRate == 44100){
            var frames = new StereoFrame16[Frames];
            for (int i = 0; i < frames.Length; i++)
            {
                frames[i].L = _reader.ReadInt16();
                frames[i].R = Channels == 2 ? _reader.ReadInt16() : frames[i].L;
            }
            return frames;
        } else if (SampleRate == 22050){
            var frames = new StereoFrame16[Frames*2];
            for (int i = 0; i < frames.Length; i+=2)
            {
                frames[i].L = _reader.ReadInt16();
                frames[i].R = Channels == 2 ? _reader.ReadInt16() : frames[i].L;
                frames[i+1].L = frames[i].L;
                frames[i+1].R = frames[i].R;
            }
            return frames;
        } else {
            throw new InvalidOperationException("Unsupported sample rate");
        }
    }
}