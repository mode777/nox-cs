using System;
using System.Drawing;
using System.Numerics;
using Microsoft.Xna.Framework;

namespace Nox.Framework;

public class QuadBuffer
{
    private const int MAX_QUADS = 16384;
    private const int MAX_INDICES = MAX_QUADS * 6;
    private int _capacity;
    private int _count;
    private Quad[] _data;
    private ushort[] _indices;
    private Buffer<Quad> _buffer;
    private Buffer<ushort> _indexBuffer;
    private bool _isDirty = false;

    public int Count => _count;

    public QuadBuffer(int capacity = 64)
    {
        _capacity = capacity;
        _count = 0;
        _data = new Quad[_capacity];
        _buffer = new Buffer<Quad>(_capacity, BufferType.VertexBuffer);
        _indices = new ushort[_capacity * 6];
        GenerateIndices();
        _indexBuffer = new Buffer<ushort>(_capacity*6, BufferType.IndexBuffer);
        _indexBuffer.Update(_indices);
        _isDirty = true;
    }

    internal Quad this[int i]
    {
        get => i < _count ? _data[i] : throw new IndexOutOfRangeException();
        set
        {
            if (i < _count) throw new IndexOutOfRangeException();
            _data[i] = value;
            _isDirty = true;
        }
    }

    public void Clear()
    {
        _count = 0;
    }

    public void Update()
    {
        if (_buffer == null || _capacity != _buffer.Size)
        {
            _buffer?.Dispose();
            _buffer = new Buffer<Quad>(_capacity, BufferType.VertexBuffer);
            _indexBuffer = new Buffer<ushort>(_capacity*6, BufferType.IndexBuffer);
            _indexBuffer.Update(_indices);
            _isDirty = true;
        }
        if(_isDirty){
            _buffer.Update(_data);
            _isDirty = false;
        }
    }

    public bool TryAddQuad(Vector2 pos, Rectangle rect, ColorRGBA color)
    {
        if(!TryGrowIfNeeded()) return false;
        float x = pos.X;
        float y = pos.Y;
        int w = rect.Width;
        int h = rect.Height;
        int ox = rect.Left;
        int oy = rect.Top;
        var quad = new Quad
        {
            a = new Vertex2D { x = x + w, y = y, u = ox + w, v = oy, color = color },
            b = new Vertex2D { x = x + w, y = y + h, u = ox + w, v = oy + h, color = color },
            c = new Vertex2D { x = x, y = y + h, u = ox, v = oy + h, color = color },
            d = new Vertex2D { x = x, y = y, u = ox, v = oy, color = color },
        };
        _data[_count] = quad;
        _count++;
        _isDirty = true;
        return true;
    }

    public bool TryAddQuadPoints(Vector2 a, Vector2 b, Vector2 c, Vector2 d, Rectangle rect, ColorRGBA color)
    {
        if(!TryGrowIfNeeded()) return false;
        int w = rect.Width;
        int h = rect.Height;
        int ox = rect.Left;
        int oy = rect.Top;
        var quad = new Quad
        {
            a = new Vertex2D { x = b.X, y = b.Y, u = ox + w, v = oy, color = color },
            b = new Vertex2D { x = c.X, y = c.Y, u = ox + w, v = oy + h, color = color },
            c = new Vertex2D { x = d.X, y = d.Y, u = ox, v = oy + h, color = color },
            d = new Vertex2D { x = a.X, y = a.Y, u = ox, v = oy, color = color },
        };
        _data[_count] = quad;
        _count++;
        _isDirty = true;
        return true;
    }

    public bool TryAddQuadTransformed(Transform2d transform, Rectangle rect, ColorRGBA color)
    {
        if(!TryGrowIfNeeded()) return false;
        var pa = transform.TransformPoint(new Vector2(rect.Width, 0));
        var pb = transform.TransformPoint(new Vector2(rect.Width, rect.Height));
        var pc = transform.TransformPoint(new Vector2(0, rect.Height));
        var pd = transform.TransformPoint(new Vector2(0, 0));
        int w = rect.Width;
        int h = rect.Height;
        int ox = rect.Left;
        int oy = rect.Top;
        var quad = new Quad
        {
            a = new Vertex2D { x = pa.X, y = pa.Y, u = ox + w, v = oy, color = color },
            b = new Vertex2D { x = pb.X, y = pb.Y, u = ox + w, v = oy + h, color = color },
            c = new Vertex2D { x = pc.X, y = pc.Y, u = ox, v = oy + h, color = color },
            d = new Vertex2D { x = pd.X, y = pd.Y, u = ox, v = oy, color = color },
        };
        _data[_count] = quad;
        _count++;
        _isDirty = true;
        return true;
    }

    private bool TryGrowIfNeeded()
    {
        if (_count == _capacity)
        {
            if (!CanGrow())
            {
                return false;
            }
            Grow();
            if (_count == _capacity) return false;
        }
        return true;
    }

    public Buffer<Quad> GetOrCreateBuffer() {
        Update();
        return _buffer;
    }

    public Buffer<ushort> GetOrCreateIndexBuffer() {
        Update();
        return _indexBuffer;
    }

    private void GenerateIndices(int start = 0){
        for (int i = start; i < _capacity; i++)
        {
            int index = i * 4;
            int offset = i * 6;
            _indices[offset+0] = (ushort)index;
            _indices[offset+1] = (ushort)(index+1);
            _indices[offset+2] = (ushort)(index+3);
            _indices[offset+3] = (ushort)(index+1);
            _indices[offset+4] = (ushort)(index+2);
            _indices[offset+5] = (ushort)(index+3);
        }
    }

    private void Grow()
    {
        var oldSize = _capacity;
        var newSize = oldSize + oldSize / 2; // grow by x1.5
        newSize = (newSize + 63) & (~63); // grow in chunks of 64.
        newSize = Math.Min(newSize, MAX_QUADS);
        Array.Resize(ref _data, newSize);
        Array.Resize(ref _indices, newSize * 6);
        _capacity = newSize;
        GenerateIndices(oldSize);
    }

    private bool CanGrow() => _capacity < MAX_QUADS;
}