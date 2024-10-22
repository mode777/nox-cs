using System;
using System.Numerics;

namespace Nox.Framework
{

    public static class FloatExtensions
    {
        public static float ToRad(this float f) => f * (MathF.PI / 180);
        public static float ToDeg(this float f) => f * (180 / MathF.PI);
    }

    public class Transform2d
    {
        private float _rotation = 0;
        private float _sin = 0;
        private float _cos = 1;
        private bool _isDirty = true;
        private Matrix3x2 _cachedMatrix;
        private Vector2 _position;
        private Vector2 _origin;
        private Vector2 _scale = Vector2.One;

        public Transform2d()
        {
        }

        public Vector2 Position {
            get => _position;
            set {
                _position = value;
                _isDirty = true;
            }
        }

        public Vector2 Origin {
            get => _origin;
            set {
                _origin = value;
                _isDirty = true;
            }
        }

        public float Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                _cos = MathF.Cos(value);
                _sin = MathF.Sin(value);
                _isDirty = true;
            }
        }

        public float RotationDeg
        {
            get => Rotation.ToDeg();
            set
            {
                Rotation = value.ToRad();
                _isDirty = true;
            }
        }

        public Vector2 Scale { 
            get => _scale;
            set {
                _scale = value;
                _isDirty = true;
            }
        }

        public Matrix3x2 GetMatrix()
        {
            CalculateMatrix();
            return _cachedMatrix;
        }

        public Matrix4x4 GetMatrix4x4()
        {
            CalculateMatrix();
            return new Matrix4x4(_cachedMatrix);
        }

        public Vector2 TransformPoint(Vector2 vec)
        {
            CalculateMatrix();
            return Vector2.Transform(vec, _cachedMatrix);
        }

        private void CalculateMatrix(){
            if (_isDirty)
            {
                _cachedMatrix = new Matrix3x2();
                _cachedMatrix.M11 = _scale.X * _cos;
                _cachedMatrix.M12 = _scale.X * _sin;

                _cachedMatrix.M21 = _scale.Y * -_sin;
                _cachedMatrix.M22 = _scale.Y * _cos;

                _cachedMatrix.M31 = -_origin.X * _scale.X * _cos + -_origin.Y * _scale.Y * -_sin + _position.X;
                _cachedMatrix.M32 = -_origin.X * _scale.X * _sin + -_origin.Y * _scale.Y * _cos + _position.Y;
                _isDirty = false;
            }
        }
    }
}