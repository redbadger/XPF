namespace Xpf.Samples.S04BasketballScoreboard
{
    using System;

    using Microsoft.Xna.Framework;

    public static class Vector3Extensions
    {
        /// <summary>
        /// Calculates the heading in degrees
        /// </summary>
        public static float CalculateHeading(this Vector3 forwards)
        {
            return MathHelper.ToDegrees((float)Math.Atan2(forwards.X, forwards.Z));
        }
    }
}