using System;
using UnityEngine;

namespace Utils.Extensions
{
    public static class MathUtils
    {
        private const double DoubleTolerance = 1E-05;
        private const float FloatTolerance = 1E-05f;

        private const int ZeroCountLimit = 8; //100 000 000

        public static double SafeDivision(this double num, double denominator)
        {
            if (denominator == 0) return 1;
            return num / denominator;
        }

        public static int GetDigitCapacity(this int value)
        {
            var digitCapacity = 1;

            for (var i = 0; i < ZeroCountLimit; i++)
            {
                digitCapacity *= 10;
                if (value <= digitCapacity) return i;
            }

            return 0;
        }

        public static float Get360Angle(this Vector2 direction)
        {
            var angle = Vector2.SignedAngle(Vector2.up, direction);

            if (angle < 0) angle = 360 + angle;

            return angle;
        }

        public static bool GetIntersectionPointCoordinates(Vector2 A1, Vector2 A2, Vector2 B1, Vector2 B2, out Vector2 point)
        {
            point = Vector2.zero;
            var tmp = (B2.x - B1.x) * (A2.y - A1.y) - (B2.y - B1.y) * (A2.x - A1.x);

            if (Mathf.Approximately(tmp, 0)) return false;

            var mu = ((A1.x - B1.x) * (A2.y - A1.y) - (A1.y - B1.y) * (A2.x - A1.x)) / tmp;

            point = new Vector2(
                B1.x + (B2.x - B1.x) * mu,
                B1.y + (B2.y - B1.y) * mu
            );

            return true;
        }

        public static Quaternion LookAt2D(this Vector2 direction)
        {
            var angleRad = Mathf.Atan2(direction.y, direction.x);
            var angleDeg = 180 / Mathf.PI * angleRad;

            return Quaternion.Euler(0, 0, angleDeg);
        }

        public static int RoundToIntNumber(this double value, int roundNumber)
        {
            return RoundToInt(value / roundNumber) * roundNumber;
        }

        public static int CeilToIntNumber(this double value, int roundNumber)
        {
            return CeilToInt(value / roundNumber) * roundNumber;
        }

        public static int FloorToIntNumber(this double value, int roundNumber)
        {
            return FloorToInt(value / roundNumber) * roundNumber;
        }

        public static int RoundToInt(this double f)
        {
            return (int) Math.Round(f, MidpointRounding.AwayFromZero);
        }

        public static int FloorToInt(this double value)
        {
            return (int) Math.Floor(value);
        }

        public static int CeilToInt(this double value)
        {
            return (int) Math.Ceiling(value);
        }

        public static bool IsAlmostEqual(this double first, double second, double tolerance = DoubleTolerance)
        {
            return Math.Abs(first - second) < tolerance;
        }

        public static bool IsAlmostEqual(this float first, float second, float tolerance = FloatTolerance)
        {
            return Math.Abs(first - second) < tolerance;
        }
    }
}