namespace Nox.Framework.Audio;

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
