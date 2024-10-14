using System.Runtime.InteropServices;

namespace Nox.Framework;

[StructLayout(LayoutKind.Sequential)]
public struct Quad
{
    public VertexPosUvCol a;
    public VertexPosUvCol b;
    public VertexPosUvCol c;
    public VertexPosUvCol d;
}

