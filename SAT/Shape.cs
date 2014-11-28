using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SAT
{
    abstract class Collider2D
    {
        protected Vector2 _Centre;
        public Vector2 Centre { get { return _Centre; } }

        protected Vector2[] _Points;
        protected Vector2[] Points { get { return _Points; } }

        public Collider2D(Vector2 Centre)
        {
            Centre = _Centre;
        }

        public abstract bool Collision(Vector2 Position, Collider2D Other, Vector2 OtherPosition);
        public abstract Vector2 GetSATMinMax(Vector2 Axis, float Offset, Vector2 Position);
    }

    class CircleCollider2D : Collider2D
    {
        protected float _Radius;
        public float Radius { get { return _Radius; } }

        public CircleCollider2D(Vector2 Centre, float Radius)
            : base(Centre)
        {
            _Radius = Radius;
            _Points = new Vector2[1] { Vector2.Zero };
        }

        public override bool Collision(Vector2 Position, Collider2D Other, Vector2 OtherPosition)
        {
            Vector2 axis = OtherPosition - Position;
            float offset = axis.Length();
            axis.Normalize();
            return CollisionSAT(axis, offset, Position, Other, OtherPosition);
        }

        protected bool CollisionSAT(Vector2 Axis, float Offset, Vector2 Position, Collider2D Other, Vector2 OtherPosition)
        {
            Vector2 minMaxA = GetSATMinMax(Axis, Offset, Position);
            Vector2 minMaxB = Other.GetSATMinMax(Axis, Offset, OtherPosition);

            //              minA    -   maxB
            //              minB    -   maxA
            float testA = minMaxA.X - minMaxB.Y;
            float testB = minMaxB.X - minMaxA.Y;
            if (testA > 0 || testB > 0)
                return false;
            return true;
        }

        public override Vector2 GetSATMinMax(Vector2 Axis, float Offset, Vector2 Position)
        {
            float point = Vector2.Dot(Axis, _Points[0] + Position + _Centre) + Offset;
            return new Vector2(point - _Radius, point + _Radius);
        }

        void MinMax()
        {
            ////  Get A's min and max
            //min1 = max1 = Vector2.Dot(axis, squareA.Points[0] + squareA.Positon);
            //for (int i = 1; i < squareA.Points.Count; ++i)
            //{
            //    float dot = Vector2.Dot(axis, squareA.Points[i] + squareA.Positon);
            //    if (dot < min1) min1 = dot;
            //    if (dot > max1) max1 = dot;
            //}
            //
            ////  Get B's min and max
            //min2 = max2 = Vector2.Dot(axis, squareB.Points[0] + squareB.Positon);
            //for (int i = 1; i < squareB.Points.Count; ++i)
            //{
            //    float dot = Vector2.Dot(axis, squareB.Points[i] + squareB.Positon);
            //    if (dot < min2) min2 = dot;
            //    if (dot > max2) max2 = dot;
            //}
            //
            ////  Add on offset
            //min1 += offset; max1 += offset;
            //min2 += offset; max2 += offset;
        }
    }

    class TriangleCollider2D : Collider2D
    {
        public TriangleCollider2D(Vector2 Centre, Vector2[] Points)
            : base(Centre)
        {
            _Points = Points;
        }

        public override bool Collision(Vector2 Position, Collider2D Other, Vector2 OtherPosition)
        {
            for (int i = 0; i < _Points.Length; ++i)
            {
                Vector2 axis = _Points[i] - _Points[(i + 1) % _Points.Length];
                float offset = axis.Length();
                axis.Normalize();

                float oldA = axis.X;
                axis.X = axis.Y;
                axis.Y = -oldA;
                if (CollisionSAT(axis, offset, Position, Other, OtherPosition) == false)
                    return false;
            }
            return true;
        }

        protected bool CollisionSAT(Vector2 Axis, float Offset, Vector2 Position, Collider2D Other, Vector2 OtherPosition)
        {
            Vector2 minMaxA = GetSATMinMax(Axis, Offset, Position);
            Vector2 minMaxB = Other.GetSATMinMax(Axis, Offset, OtherPosition);

            //              minA    -   maxB
            //              minB    -   maxA
            float testA = minMaxA.X - minMaxB.Y;
            float testB = minMaxB.X - minMaxA.Y;
            if (testA > 0 || testB > 0)
                return false;
            return true;
        }

        public override Vector2 GetSATMinMax(Vector2 Axis, float Offset, Vector2 Position)
        {
            float min, max;
            min = max = Vector2.Dot(Axis, _Points[0] + Position + _Centre);
            for (int i = 1; i < _Points.Length; ++i)
            {
                float dot = Vector2.Dot(Axis, _Points[i] + Position + _Centre);
                if (dot < min) min = dot;
                if (dot > max) max = dot;
            }
            return new Vector2(min + Offset, max + Offset);
        }
    }
}
