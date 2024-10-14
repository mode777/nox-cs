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
        if (_count == _capacity)
        {
            if (!CanGrow())
            {
                return false;
            }
            Grow();
            if(_count == _capacity) return false;
        }
        float x = pos.X;
        float y = pos.Y;
        int w = rect.Width;
        int h = rect.Height;
        int ox = rect.Left;
        int oy = rect.Top;
        var quad = new Quad
        {
            a = new VertexPosUvCol { x = x + w, y = y, u = ox + w, v = oy, color = color },
            b = new VertexPosUvCol { x = x + w, y = y + h, u = ox + w, v = oy + h, color = color },
            c = new VertexPosUvCol { x = x, y = y + h, u = ox, v = oy + h, color = color },
            d = new VertexPosUvCol { x = x, y = y, u = ox, v = oy, color = color },
        };
        _data[_count] = quad;
        _count++;
        _isDirty = true;
        return true;
    }

    public Buffer<Quad> GetBuffer() {
        Update();
        return _buffer;
    }

    public Buffer<ushort> GetIndexBuffer() {
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