using System.Numerics;
using Microsoft.Xna.Framework;
using Nox.Framework;

namespace Nox.Samples;

public class MeshGame : Game
{
    private Texture2D _texture;
    private Buffer<Vertex2D> _buffer;
    private Buffer<ushort> _indexBuffer;

    public override void Init()
    {
        var image = new Image(1,1,4);
        image.SetPixel(0,0,ColorRGBA.White);
        _texture = Texture2D.FromImage(image);
        var data = new Vertex2D[3] {
            new Vertex2D(0,0,0,0,ColorRGBA.Red),
            new Vertex2D(0,GraphicsDevice.Size.Y,0,0,ColorRGBA.Lime),
            new Vertex2D(GraphicsDevice.Size.X,0,0,0,ColorRGBA.Blue),
        };
        var indices = new ushort[3] {0,1,2};
        _buffer = Framework.Buffer.FromData(data, BufferType.VertexBuffer);
        _indexBuffer = Framework.Buffer.FromData(indices, BufferType.IndexBuffer);
        base.Init();
    }

    public override void Update(double deltaTime)
    {
        base.Update(deltaTime);
    }

    public override void Render()
    {
        Renderer2D.Begin();
        Renderer2D.Draw(_texture, _buffer, _indexBuffer, 0, 3, Matrix4x4.Identity);
        Renderer2D.End();
        base.Render();
    }
}