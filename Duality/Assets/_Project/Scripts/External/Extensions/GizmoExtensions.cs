using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magthylius
{
    public static class GizmosEx
    {
        public static void DrawArrow(Vector3 position, Vector3 direction, float arrowLength = 1f, float arrowHeadLength = 0.3f, float arrowHeadAngle = 20f)
        {
            direction *= arrowLength;
            Gizmos.DrawRay(position, direction);
            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Gizmos.DrawRay(position + direction, right * arrowHeadLength);
            Gizmos.DrawRay(position + direction, left * arrowHeadLength);
        }

        public static void DrawCircle2D(Vector3 position, float radius, int resolution = 10)
        {
            resolution = Mathf.Max(resolution, 3);
            float angle = 2 * Mathf.PI / resolution;

            Vector3[] points = new Vector3[resolution];
            points[0] = Vector3.up * radius + position;
            for (int i = 1; i < resolution; i++)
            {
                points[i] = new Vector3(Mathf.Sin(angle * i), Mathf.Cos(angle * i), 0f) * radius + position;
                Gizmos.DrawLine(points[i - 1], points[i]);
            }

            Gizmos.DrawLine(points[resolution - 1], points[0]);
        }

        public static void DrawBox2D(Vector3 position, Vector2 size)
        {
            float halfWidth = size.x * 0.5f;
            float halfHeight = size.y * 0.5f;

            Vector3[] points =
            {
                position + new Vector3(-halfWidth, halfHeight, 0f),
                position + new Vector3(halfWidth, halfHeight, 0f),
                position + new Vector3(halfWidth, -halfHeight, 0f),
                position + new Vector3(-halfWidth, -halfHeight, 0f)
            };

            DrawPolyLine(points, true);
        }

        public static void DrawPolyLine(Vector3[] points, bool looped)
        {
            for (int i = 0; i < points.Length - 1; i++)
                Gizmos.DrawLine(points[i], points[i + 1]);

            if (looped) Gizmos.DrawLine(points[points.Length - 1], points[0]);
        }
    }
    
}
