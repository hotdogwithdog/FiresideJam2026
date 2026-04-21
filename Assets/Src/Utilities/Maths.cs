using System;
using UnityEngine;

namespace Utilities
{
    public static class Maths
    {
        /// <summary>
        /// Rotate a point in 2D by an angle, the rotation direction is counter-clockwise for positive angles
        /// </summary>
        /// <param name="angle"> The angle to rotate in radians </param>
        /// <param name="point"> The point to rotate </param>
        /// <returns></returns>
        public static Vector2 Rotate2D(float angle, Vector2 point) 
        {
            return new Vector2(MathF.Cos(angle) * point.x - MathF.Sin(angle) * point.y, MathF.Sin(angle) * point.x + MathF.Cos(angle) * point.y);
        }
    }
}