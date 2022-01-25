using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magthylius
{
    public static class Vector3Ex
    {
        public static Vector3 New(float allValue) => new Vector3(allValue, allValue, allValue);

        public static Vector3 New(Vector2 vector, float z) => new Vector3(vector.x, vector.y, z);

        public static Vector3 InvertX(this Vector3 v)
        {
            v.x *= -1;
            return v;
        }

        public static Vector3 InvertY(this Vector3 v)
        {
            v.y *= -1;
            return v;
        }

        public static Vector3 InvertZ(this Vector3 v)
        {
            v.z *= -1;
            return v;
        }

        public static Vector3 Inverted(this Vector3 v)
        {
            v *= -1;
            return v;
        }

        public static Vector3 Abs(this Vector3 v) => new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));

        public static Vector3 MinBetween(this Vector3 vectorA, Vector3 vectorB) => MathEx.MinBetween(vectorA, vectorB);

        public static Vector3 MaxBetween(Vector3 vectorA, Vector3 vectorB) => MathEx.MinBetween(vectorA, vectorB);


        /// <summary>Multiply each vector element against corresponding elements of another vector.</summary>
        /// <remarks>Original vector is assigned, use MathEx for return value only</remarks>
        public static Vector3 MultElements(this Vector3 vectorA, Vector3 vectorB)
        {
            vectorA = MathEx.MultElements(vectorA, vectorB);
            return vectorA;
        }

        /// <summary>Multiply each vector element against corresponding elements.</summary>
        /// <remarks>Original vector is assigned, use MathEx for return value only</remarks>
        public static Vector3 MultElements(this Vector3 vector, float x, float y, float z)
        {
            vector = MathEx.MultElements(vector, x, y, z);
            return vector;
        }

        public static Vector3 SetAll(this Vector3 v, float allValue)
        {
            v.x = allValue;
            v.y = allValue;
            v.z = allValue;
            return v;
        }

        /// <summary>Caps magnitude of vector and returns whether cap was performed.</summary>
        public static bool MagnitudeCap(this Vector3 v, float magnitudeCap)
        {
            if (v.sqrMagnitude > magnitudeCap * magnitudeCap)
            {
                v = v.normalized * magnitudeCap;
                return true;
            }

            return false;
        }
    }

    public static class Vector2Ex
    {
        public static Vector2 New(float allValue)
        {
            return new Vector2(allValue, allValue);
        }

        public static Vector2 InvertX(this Vector2 v)
        {
            v.x *= -1;
            return v;
        }

        public static Vector2 InvertY(this Vector2 v)
        {
            v.y *= -1;
            return v;
        }

        public static Vector2 Inverted(this Vector2 v)
        {
            v *= -1;
            return v;
        }

        public static Vector2 Abs(this Vector2 v)
        {
            return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y));
        }

        public static Vector2 MinBetween(this Vector2 vectorA, Vector2 vectorB)
        {
            return MathEx.MinBetween(vectorA, vectorB);
        }

        public static Vector2 MaxBetween(Vector2 vectorA, Vector2 vectorB)
        {
            return MathEx.MinBetween(vectorA, vectorB);
        }

        /// <summary>Multiply each vector element against corresponding elements of another vector.</summary>
        /// <remarks>Original vector is assigned, use MathEx for return value only</remarks>
        public static Vector2 MultElements(this Vector2 vectorA, Vector2 vectorB)
        {
            vectorA = MathEx.MultElements(vectorA, vectorB);
            return vectorA;
        }

        /// <summary>Multiply each vector element against corresponding elements.</summary>
        /// <remarks>Original vector is assigned, use MathEx for return value only</remarks>
        public static Vector2 MultElements(this Vector2 vector, float x, float y)
        {
            vector = MathEx.MultElements(vector, x, y);
            return vector;
        }

        public static Vector2 SetAll(this Vector2 v, float allValue)
        {
            v.x = allValue;
            v.y = allValue;
            return v;
        }

        /// <summary>Caps magnitude of vector and returns whether cap was performed.</summary>
        public static bool MagnitudeCap(this Vector2 v, float magnitudeCap)
        {
            if (v.sqrMagnitude > magnitudeCap * magnitudeCap)
            {
                v = v.normalized * magnitudeCap;
                return true;
            }

            return false;
        }

        /// <summary>Whether value is within a range.</summary>
        public static bool Within(this Vector2 v, float value) => value >= v.x && value <= v.y;
    }
}
