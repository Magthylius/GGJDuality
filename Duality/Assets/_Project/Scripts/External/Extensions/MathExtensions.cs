using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magthylius
{
    public static class MathEx
    {
        public const float FullAngleDeg = 360f;
        public const float FullAngleRad = 2 * Mathf.PI;

        public static float Square(float f) => f * f;
        public static float Cube(float f) => f * f * f;
        public static float AtanDeg(float f) => Mathf.Atan(f) * Mathf.Rad2Deg;
        public static float Atan2Deg(float y, float x) => Mathf.Atan2(y, x) * Mathf.Rad2Deg;

        public static float Lerp(Vector2 range, float t) => Mathf.Lerp(range.x, range.y, t);

        /// <summary>Float lerp with value snapping.</summary>
        /// <param name="tSnap">t step to snap.</param>
        /// <returns>Snapped value to b when t larger than tSnap.</returns>
        public static float LerpSnap(float a, float b, float t, float tSnap)
        {
            float value = Mathf.Lerp(a, b, t);
            if (t > tSnap) return b;
            return value;
        }

        public static Vector3 LerpSnap(Vector3 a, Vector3 b, float t, float tSnap)
        {
            Vector3 value = Vector3.Lerp(a, b, t);
            if (t > tSnap) return b;
            return value;
        }

        public static float InverseLerp(Vector2 range, float value) => Mathf.InverseLerp(range.x, range.y, value);

        public static bool Within(float value, float minRange, float maxRange) => value >= minRange && value <= maxRange;
        public static bool Within(float value, Vector2 range) => value >= range.x && value <= range.y;

        public static bool Approximate(float value, float comparison, float approximation) => Mathf.Abs(comparison - value) < approximation;

        public static float NormalizeAngle(float angleInDegrees) => angleInDegrees %= FullAngleDeg;
        public static float RelateAngle(float angleInDegrees, float mirrorAngleAxis = 180f)
        {
            float angle = NormalizeAngle(angleInDegrees);
            if (angle > mirrorAngleAxis) return FullAngleDeg - angle;
            if (angle < -mirrorAngleAxis) return FullAngleDeg + angle;

            return angle;
        }

        public static float AbsoluteAngle(float angle)
        {
            angle = NormalizeAngle(angle);
            return angle < 0 ? angle + FullAngleDeg : angle;
        }

        #region Vector Math
        public static Vector3 MinBetween(Vector3 vectorA, Vector3 vectorB) =>
            new Vector3(Mathf.Min(vectorA.x, vectorB.x), Mathf.Min(vectorA.y, vectorB.y), Mathf.Min(vectorA.z, vectorB.z));

        public static Vector2 MinBetween(Vector2 vectorA, Vector2 vectorB) =>
            new Vector2(Mathf.Min(vectorA.x, vectorB.x), Mathf.Min(vectorA.y, vectorB.y));

        public static Vector3 MaxBetween(Vector3 vectorA, Vector3 vectorB) =>
            new Vector3(Mathf.Max(vectorA.x, vectorB.x), Mathf.Max(vectorA.y, vectorB.y), Mathf.Max(vectorA.z, vectorB.z));

        public static Vector2 MaxBetween(Vector2 vectorA, Vector2 vectorB) =>
            new Vector2(Mathf.Max(vectorA.x, vectorB.x), Mathf.Max(vectorA.y, vectorB.y));

        /// <summary>Multiply each vector element against corresponding elements of another vector.</summary>
        public static Vector3 MultElements(Vector3 vectorA, Vector3 vectorB) =>
            new Vector3(vectorA.x * vectorB.x, vectorA.y * vectorB.y, vectorA.z * vectorB.z);

        /// <summary>Multiply each vector element against corresponding elements.</summary>
        public static Vector3 MultElements(Vector3 vector, float x, float y, float z) => new Vector3(vector.x* x, vector.y* y, vector.z* z);

        /// <summary>Multiply each vector element against corresponding elements of another vector.</summary>
        public static Vector2 MultElements(Vector2 vectorA, Vector2 vectorB) => new Vector2(vectorA.x * vectorB.x, vectorA.y * vectorB.y);

        /// <summary>Multiply each vector element against corresponding elements.</summary>
        public static Vector2 MultElements(Vector2 vector, float x, float y) => new Vector2(vector.x * x, vector.y * y);

        /// <summary>Whether a vector magnitude is in range.</summary>
        public static bool InRange(Vector3 vectorA, float range) => vectorA.sqrMagnitude < range * range;

        /// <summary>Whether if vectorA's magnitude is smaller than vectorB's.</summary>
        public static bool InRange(Vector3 vectorA, Vector3 vectorB) => vectorA.sqrMagnitude < vectorB.sqrMagnitude;

        /// <summary>Whether the distance between vectorA and vectorB are in range.</summary>
        public static bool InRange(Vector3 vectorA, Vector3 vectorB, float range) => (vectorB - vectorA).sqrMagnitude < range * range;

        /// <summary>Whether the distance between vectorA and vectorB are in range.</summary>
        /// <param name="distance">Distance data output.</param>
        public static bool InRange(Vector3 vectorA, Vector3 vectorB, float range, ref float distance)
        {
            distance = Vector3.Distance(vectorA, vectorB);
            return distance < range;
        }

        public static float DistanceXY(Vector3 vectorA, Vector3 vectorB) =>
            Mathf.Sqrt(MathEx.Square(vectorA.x - vectorB.x) + MathEx.Square(vectorA.y - vectorB.y));

        public static float DistanceXZ(Vector3 vectorA, Vector3 vectorB) =>
            Mathf.Sqrt(MathEx.Square(vectorA.x - vectorB.x) + MathEx.Square(vectorA.z - vectorB.z));

        public static float DistanceYZ(Vector3 vectorA, Vector3 vectorB) =>
            Mathf.Sqrt(MathEx.Square(vectorA.y - vectorB.y) + MathEx.Square(vectorA.z - vectorB.z));

        public static Vector3 MagnitudeCap(Vector3 vector, float magnitudeCap)
        {
            if (vector.sqrMagnitude > MathEx.Square(magnitudeCap))
                vector = vector.normalized * magnitudeCap;

            return vector;
        }
        #endregion
    }
}