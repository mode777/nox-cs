namespace Nox.Framework.Audio;

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
