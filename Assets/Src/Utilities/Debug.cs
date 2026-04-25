using System;
using UnityEngine;

namespace Utilities
{
    public static class Debug
    {
        public static void DrawPoint(Vector2 point, float radius, Color color, float time = 1f, int numberOfPoints = 6)
        {
            #if UNITY_EDITOR
            Vector2 startPoint;
            Vector2 endPoint;
            float angleInterval = 2f*MathF.PI /  numberOfPoints; // in radians
            float angle = 0f;
            for (int i = 0; i < numberOfPoints; ++i)
            {
                startPoint = new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * radius + point;
                endPoint = new Vector2(MathF.Cos(angle + angleInterval), MathF.Sin(angle + angleInterval)) * radius + point;
                UnityEngine.Debug.DrawLine(startPoint, endPoint, color, time);
                angle += angleInterval;
            }
            #endif
        }
    }
}