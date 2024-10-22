using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace Nox.Framework;


[StructLayout(LayoutKind.Sequential)]
public struct Vertex2D
{
    public Vertex2D(float x, float y, float u, float v, ColorRGBA color)
    {
        this.x = x;
        this.y = y;
        this.u = u;
        this.v = v;
        this.color = color;
    }
    
    public float x;
    public float y;
    public float u;
    public float v;
    public ColorRGBA color;
}

