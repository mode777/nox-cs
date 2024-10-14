using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace Nox.Framework;


[StructLayout(LayoutKind.Sequential)]
public struct VertexPosUvCol
{
    public float x;
    public float y;
    public float u;
    public float v;
    public ColorRGBA color;
}

