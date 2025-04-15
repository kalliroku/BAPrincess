using System;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

namespace Emi.Unity
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// <see cref="sides"/> 값에 따라서 활성화 되는 속성들 목록
    /// ┌─────────────┬──────────┬───────────┬──────────────┬─────────────┐
    /// │             │ Top-left │ Top-right │ Bottom-right │ Bottom-left │
    /// ├─────────────┼──────────┼───────────┼──────────────┼─────────────┤
    /// │ None        │ Radius 1 │ Radius 2  │ Radius 3     │ Radius 4    │
    /// │ All         │ Radius 1 │ Radius 1  │ Radius 1     │ Radius 1    │
    /// │ Horizontal  │ Radius 1 │ Radius 2  │ Radius 2     │ Radius 1    │
    /// │ Vertical    │ Radius 1 │ Radius 1  │ Radius 2     │ Radius 2    │
    /// │ Left Side   │ Radius 1 │ Radius 2  │ Radius 3     │ Radius 1    │
    /// │ Top Side    │ Radius 1 │ Radius 1  │ Radius 2     │ Radius 3    │
    /// │ Right Side  │ Radius 1 │ Radius 2  │ Radius 2     │ Radius 3    │
    /// │ Bottom Side │ Radius 1 │ Radius 2  │ Radius 3     │ Radius 3    │
    /// └─────────────┴──────────┴───────────┴──────────────┴─────────────┘
    /// </remarks>
    [AddComponentMenu("UI/Effects/Rounded Corners")]
    public sealed class RoundedCorners : BaseMeshEffect, ICanvasRaycastFilter
    {
        [SerializeField]
        private Sides sides = Sides.All;
        [SerializeField]
        private bool isElliptical = false;
        [SerializeField]
        private Radius radius1 = Radius.Zero;
        [SerializeField]
        private Radius radius2 = Radius.Zero;
        [SerializeField]
        private Radius radius3 = Radius.Zero;
        [SerializeField]
        private Radius radius4 = Radius.Zero;

        public override void ModifyMesh(VertexHelper vh)
        {
            if (IsActive())
            {
                var graphic = base.graphic;
                if (graphic is Image image)
                {
                    if (image.type == Image.Type.Simple)
                    {
                        var rect = graphic.GetPixelAdjustedRect();
                        var uv = image.overrideSprite != null ? DataUtility.GetOuterUV(image.overrideSprite) : Vector4.zero;
                        var uvRect = Rect.MinMaxRect(uv.x, uv.y, uv.z, uv.w);
                        GenerateSprite(vh, rect, uvRect);
                    }
                }
                else if (graphic is RawImage rawImage)
                {
                    var rect = graphic.GetPixelAdjustedRect();
                    GenerateSprite(vh, rect, rawImage.uvRect);
                }
            }
        }

        private void GenerateSprite(VertexHelper vh, Rect rect, Rect uvRect)
        {
            var topLeftCorner = GetTopLeftCorner(rect);
            var topRightCorner = GetTopRightCorner(rect);
            var bottomRightCorner = GetBottomRightCorner(rect);
            var bottomLeftCorner = GetBottomLeftCorner(rect);
            if (topLeftCorner.IsRounded == false &&
                topRightCorner.IsRounded == false &&
                bottomRightCorner.IsRounded == false &&
                bottomLeftCorner.IsRounded == false)
                return;

            var color32 = (Color32)graphic.color;

            var center = rect.center;
            var uvCenter = uvRect.center;

            vh.Clear();
            vh.AddVert(new Vector3(center.x, center.y), color32, uvCenter);

            if (topLeftCorner.IsRounded)
            {
                var density = CalculateDensity();
                for (var i = 0; i <= density; i++)
                {
                    var angle = Mathf.Lerp(Mathf.PI / 2.0f, Mathf.PI, (float)i / density);
                    var x = topLeftCorner.Center.x + (Mathf.Cos(angle) * topLeftCorner.Radius.x);
                    var y = topLeftCorner.Center.y + (Mathf.Sin(angle) * topLeftCorner.Radius.y);
                    var uv = Rect.NormalizedToPoint(uvRect, Rect.PointToNormalized(rect, new Vector2(x, y)));

                    var index = vh.currentVertCount;
                    vh.AddVert(new Vector3(x, y), color32, uv);
                    vh.AddTriangle(0, index + 1, index);
                }
            }
            else
            {
                var index = vh.currentVertCount;
                vh.AddVert(new Vector3(rect.xMin, rect.yMax), color32, new Vector2(uvRect.xMin, uvRect.yMax));
                vh.AddTriangle(0, index + 1, index);
            }

            if (bottomLeftCorner.IsRounded)
            {
                var density = CalculateDensity();
                for (var i = 0; i <= density; i++)
                {
                    var angle = Mathf.Lerp(Mathf.PI, Mathf.PI * 3.0f / 2.0f, (float)i / density);
                    var x = bottomLeftCorner.Center.x + (Mathf.Cos(angle) * bottomLeftCorner.Radius.x);
                    var y = bottomLeftCorner.Center.y + (Mathf.Sin(angle) * bottomLeftCorner.Radius.y);
                    var uv = Rect.NormalizedToPoint(uvRect, Rect.PointToNormalized(rect, new Vector2(x, y)));

                    var index = vh.currentVertCount;
                    vh.AddVert(new Vector3(x, y), color32, uv);
                    vh.AddTriangle(0, index + 1, index);
                }
            }
            else
            {
                var index = vh.currentVertCount;
                vh.AddVert(new Vector3(rect.xMin, rect.yMin), color32, new Vector2(uvRect.xMin, uvRect.yMin));
                vh.AddTriangle(0, index + 1, index);
            }

            if (bottomRightCorner.IsRounded)
            {
                var density = CalculateDensity();
                for (var i = 0; i <= density; i++)
                {
                    var angle = Mathf.Lerp(Mathf.PI * 3.0f / 2.0f, Mathf.PI * 2.0f, (float)i / density);
                    var x = bottomRightCorner.Center.x + (Mathf.Cos(angle) * bottomRightCorner.Radius.x);
                    var y = bottomRightCorner.Center.y + (Mathf.Sin(angle) * bottomRightCorner.Radius.y);
                    var uv = Rect.NormalizedToPoint(uvRect, Rect.PointToNormalized(rect, new Vector2(x, y)));

                    var index = vh.currentVertCount;
                    vh.AddVert(new Vector3(x, y), color32, uv);
                    vh.AddTriangle(0, index + 1, index);
                }
            }
            else
            {
                var index = vh.currentVertCount;
                vh.AddVert(new Vector3(rect.xMax, rect.yMin), color32, new Vector2(uvRect.xMax, uvRect.yMin));
                vh.AddTriangle(0, index + 1, index);
            }

            if (topRightCorner.IsRounded)
            {
                var density = CalculateDensity();
                for (var i = 0; i <= density; i++)
                {
                    var angle = Mathf.Lerp(0.0f, Mathf.PI / 2.0f, (float)i / density);
                    var x = topRightCorner.Center.x + (Mathf.Cos(angle) * topRightCorner.Radius.x);
                    var y = topRightCorner.Center.y + (Mathf.Sin(angle) * topRightCorner.Radius.y);
                    var uv = Rect.NormalizedToPoint(uvRect, Rect.PointToNormalized(rect, new Vector2(x, y)));

                    var index = vh.currentVertCount;
                    vh.AddVert(new Vector3(x, y), color32, uv);

                    if (i < density)
                        vh.AddTriangle(0, index + 1, index);
                    else
                        vh.AddTriangle(0, 1, index);
                }
            }
            else
            {
                var index = vh.currentVertCount;
                vh.AddVert(new Vector3(rect.xMax, rect.yMax), color32, new Vector2(uvRect.xMax, uvRect.yMax));
                vh.AddTriangle(0, 1, index);
            }
        }

        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            var graphic = base.graphic;
            if (graphic == null)
                return false;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(graphic.rectTransform, sp, eventCamera, out var localPoint) == false)
                return false;

            var rect = graphic.GetPixelAdjustedRect();

            if (localPoint.x > rect.center.x)
            {
                if (localPoint.y > rect.center.y)
                    return IsRaycastLocationValid(GetTopRightCorner(rect), localPoint, +1.0f, +1.0f);
                else
                    return IsRaycastLocationValid(GetBottomRightCorner(rect), localPoint, +1.0f, -1.0f);
            }
            else
            {
                if (localPoint.y > rect.center.y)
                    return IsRaycastLocationValid(GetTopLeftCorner(rect), localPoint, -1.0f, +1.0f);
                else
                    return IsRaycastLocationValid(GetBottomLeftCorner(rect), localPoint, -1.0f, -1.0f);
            }

            static bool IsRaycastLocationValid(Corner corner, Vector2 localPoint, float xScale, float yScale)
            {
                var dx = (localPoint.x - corner.Center.x) * xScale;
                var dy = (localPoint.y - corner.Center.y) * yScale;
                var rx = corner.Radius.x;
                var ry = corner.Radius.y;
                return dx <= 0.0f || dy <= 0.0f || ((dx * dx) / (rx * rx) + (dy * dy) / (ry * ry)) <= 1.0f;
            }
        }

        private Vector2 GetTopLeftRadius(Vector2 size)
        {
            return Evaluate(radius1, size);
        }

        private Vector2 GetTopRightRadius(Vector2 size)
        {
            switch (sides)
            {
                case Sides.All:
                case Sides.Vertical:
                case Sides.TopSide:
                    return Evaluate(radius1, size);
                default:
                    return Evaluate(radius2, size);
            }
        }

        private Vector2 GetBottomRightRadius(Vector2 size)
        {
            switch (sides)
            {
                case Sides.All:
                    return Evaluate(radius1, size);
                case Sides.Horizontal:
                case Sides.Vertical:
                case Sides.TopSide:
                case Sides.RightSide:
                    return Evaluate(radius2, size);
                default:
                    return Evaluate(radius3, size);
            }
        }

        private Vector2 GetBottomLeftRadius(Vector2 size)
        {
            switch (sides)
            {
                case Sides.All:
                case Sides.Horizontal:
                case Sides.LeftSide:
                    return Evaluate(radius1, size);
                case Sides.Vertical:
                    return Evaluate(radius2, size);
                case Sides.TopSide:
                case Sides.RightSide:
                case Sides.BottomSide:
                    return Evaluate(radius3, size); ;
                default:
                    return Evaluate(radius4, size);
            }
        }

        private Corner GetTopLeftCorner(Rect rect)
        {
            var radius = GetTopLeftRadius(rect.size);
            return new Corner(new Vector2(rect.xMin + radius.x, rect.yMax - radius.y), radius);
        }

        private Corner GetTopRightCorner(Rect rect)
        {
            var radius = GetTopRightRadius(rect.size);
            return new Corner(new Vector2(rect.xMax - radius.x, rect.yMax - radius.y), radius);
        }

        private Corner GetBottomRightCorner(Rect rect)
        {
            var radius = GetBottomRightRadius(rect.size);
            return new Corner(new Vector2(rect.xMax - radius.x, rect.yMin + radius.y), radius);
        }

        private Corner GetBottomLeftCorner(Rect rect)
        {
            var radius = GetBottomLeftRadius(rect.size);
            return new Corner(new Vector2(rect.xMin + radius.x, rect.yMin + radius.y), radius);
        }

        private Vector2 Evaluate(Radius radius, Vector2 size)
        {
            if (isElliptical)
            {
                var x = Mathf.Min(radius.X.AbsoluteValue + (radius.X.RelativeValue * size.x), size.x / 2.0f);
                var y = Mathf.Min(radius.Y.AbsoluteValue + (radius.Y.RelativeValue * size.y), size.y / 2.0f);
                return (x > 0.0f && y > 0.0f) ? new Vector2(x, y) : Vector2.zero;
            }
            else
            {
                var value = Mathf.Min(radius.X.AbsoluteValue + (radius.X.RelativeValue * size.x), Mathf.Min(size.x / 2.0f, size.y / 2.0f));
                return (value > 0.0f) ? new Vector2(value, value) : Vector2.zero;
            }
        }

        private int CalculateDensity()
        {
            return 16;
        }

        public enum Sides : byte
        {
            None = 0,
            All = LeftSide | TopSide | RightSide | BottomSide,
            Horizontal = LeftSide | RightSide,
            Vertical = TopSide | BottomSide,
            LeftSide = 1 << 0,
            TopSide = 1 << 1,
            BottomSide = 1 << 2,
            RightSide = 1 << 3,
        }

        [Serializable]
        private struct Component : IEquatable<Component>
        {
            public static readonly Component Zero = new Component(0.0f, 0.0f);

            [SerializeField]
            private float absoluteValue;
            [SerializeField]
            private float relativeValue;

            public float AbsoluteValue => Mathf.Max(absoluteValue, 0.0f);
            public float RelativeValue => Mathf.Clamp01(relativeValue);

            public Component(float absoluteValue, float relativeValue)
            {
                this.absoluteValue = absoluteValue;
                this.relativeValue = relativeValue;
            }

            public bool Equals(Component other) => Equals(this, other);
            public override bool Equals(object obj) => obj is Component other && Equals(other);
            public override int GetHashCode() => HashCode.Combine(typeof(Component), AbsoluteValue, RelativeValue);
            public override string ToString() => base.ToString();

            public static bool Equals(Component left, Component right)
                => left.AbsoluteValue == right.AbsoluteValue && left.RelativeValue == right.RelativeValue;

            public static bool operator ==(Component left, Component right) => Equals(left, right);
            public static bool operator !=(Component left, Component right) => Equals(left, right) == false;
        }

        [Serializable]
        private struct Radius : IEquatable<Radius>
        {
            public static readonly Radius Zero = new Radius(Component.Zero, Component.Zero);

            [SerializeField]
            private Component x;
            [SerializeField]
            private Component y;

            public Component X => x;
            public Component Y => y;

            public Radius(Component x, Component y)
            {
                this.x = x;
                this.y = y;
            }

            public bool Equals(Radius other) => Equals(this, other);
            public override bool Equals(object obj) => obj is Radius other && Equals(other);
            public override int GetHashCode() => HashCode.Combine(typeof(Radius), x, y);
            public override string ToString() => base.ToString();

            public static bool Equals(Radius left, Radius right)
                => left.x == right.x && left.y == right.y;

            public static bool operator ==(Radius left, Radius right) => Equals(left, right);
            public static bool operator !=(Radius left, Radius right) => Equals(left, right) == false;
        }

        private struct Corner : IEquatable<Corner>
        {
            public readonly Vector2 Center;
            public readonly Vector2 Radius;

            public bool IsRounded => Radius.x != 0.0f;

            public Corner(Vector2 center, Vector2 radius)
            {
                this.Center = center;
                this.Radius = radius;
            }

            public bool Equals(Corner other) => Equals(this, other);
            public override bool Equals(object obj) => obj is Corner other && Equals(other);
            public override int GetHashCode() => HashCode.Combine(typeof(Corner), Center, Radius);
            public override string ToString() => base.ToString();

            public static bool Equals(Corner left, Corner right)
                => left.Center == right.Center && left.Radius == right.Radius;

            public static bool operator ==(Corner left, Corner right) => Equals(left, right);
            public static bool operator !=(Corner left, Corner right) => Equals(left, right) == false;
        }
    }
}
