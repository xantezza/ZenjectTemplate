using UnityEngine;

namespace Utils.Extensions
{
    public static class VectorExtensions
    {
        public static float GetRandomValue(this Vector2 vector)
        {
            return Random.Range(vector.x, vector.y);
        }

        public static Vector3 ToVector3(this Vector2 vector)
        {
            return new Vector3(vector.x, vector.y, 0);
        }

        public static Vector2 ToVector2(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.y);
        }

        public static Vector3 Multiply(this Vector3 vector, Vector3 multiplyVector)
        {
            var newVector = new Vector3(vector.x, vector.y, vector.z);

            newVector.x *= multiplyVector.x;
            newVector.y *= multiplyVector.y;
            newVector.z *= multiplyVector.z;

            return newVector;
        }
    }
}