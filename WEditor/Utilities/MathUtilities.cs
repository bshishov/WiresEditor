#region

using System;

#endregion

namespace WEditor.Utilities
{
    public static class MathUtilities
    {
        #region Methods

        public static float Snap(float value, float gridsize)
        {
            return gridsize*(float) Math.Floor(value/gridsize);
        }

        #endregion
    }
}