using UnityEngine;

namespace Utils.Extensions
{
    public static class RectExtensions
    {
        public static bool PointOnBorder(this Rect rect, Vector2 outerPoint, out Vector2 pointOnBorder)
        {
            pointOnBorder = new Vector2(float.MaxValue, float.MaxValue);

            var isAssigned = false;

            Vector2 CheckDistanceAndAssign(Vector2 oldPoint, Vector2 newPoint)
            {
                if (!rect.Contains(newPoint + (rect.center - newPoint).normalized * 0.1f)) return oldPoint;
                return Vector2.Distance(oldPoint, outerPoint) > Vector2.Distance(newPoint, outerPoint) ? newPoint : oldPoint;
            }

            if (rect.Contains(outerPoint)) return false;
            if (MathUtils.GetIntersectionPointCoordinates(rect.X0Y0(), rect.X0Y1(), rect.center, outerPoint,
                    out var currentPoint))
            {
                isAssigned = true;
                pointOnBorder = CheckDistanceAndAssign(pointOnBorder, currentPoint);
            }

            if (MathUtils.GetIntersectionPointCoordinates(rect.X0Y1(), rect.X1Y1(), rect.center, outerPoint,
                    out currentPoint))
            {
                isAssigned = true;
                pointOnBorder = CheckDistanceAndAssign(pointOnBorder, currentPoint);
            }

            if (MathUtils.GetIntersectionPointCoordinates(rect.X1Y1(), rect.X1Y0(), rect.center, outerPoint,
                    out currentPoint))
            {
                isAssigned = true;
                pointOnBorder = CheckDistanceAndAssign(pointOnBorder, currentPoint);
            }

            if (MathUtils.GetIntersectionPointCoordinates(rect.X0Y0(), rect.X1Y0(), rect.center, outerPoint,
                    out currentPoint))
            {
                isAssigned = true;
                pointOnBorder = CheckDistanceAndAssign(pointOnBorder, currentPoint);
            }

            return isAssigned;
        }

        public static Vector2 LeftBottomPoint(this Rect rect)
        {
            return new Vector2(rect.center.x - rect.width / 2, rect.center.y - rect.height / 2);
        }

        public static Vector2 LeftTopPoint(this Rect rect)
        {
            return new Vector2(rect.center.x - rect.width / 2, rect.center.y + rect.height / 2);
        }

        public static Vector2 RightBottomPoint(this Rect rect)
        {
            return new Vector2(rect.center.x + rect.width / 2, rect.center.y - rect.height / 2);
        }

        public static Vector2 RightTopPoint(this Rect rect)
        {
            return new Vector2(rect.center.x + rect.width / 2, rect.center.y + rect.height / 2);
        }

        public static Vector2 X0Y0(this Rect rect)
        {
            return new Vector2(rect.xMin, rect.yMin);
        }

        public static Vector2 X0Y1(this Rect rect)
        {
            return new Vector2(rect.xMin, rect.yMax);
        }

        public static Vector2 X1Y0(this Rect rect)
        {
            return new Vector2(rect.xMax, rect.yMin);
        }

        public static Vector2 X1Y1(this Rect rect)
        {
            return new Vector2(rect.xMax, rect.yMax);
        }
    }
}