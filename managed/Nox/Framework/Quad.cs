using System.Runtime.InteropServices;

namespace Nox.Framework;

[StructLayout(LayoutKind.Sequential)]
public struct Quad
{
    public Vertex2D a;
    public Vertex2D b;
    public Vertex2D c;
    public Vertex2D d;
}

