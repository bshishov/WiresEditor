#region

using System;

#endregion

namespace WEditor.Utilities
{
    public class Camera
    {
        #region Fields

        public float MaxZoom = 10f;
        public float MinZoom = 0.1f;
        public float X = 0;
        public float Y = 0;

        public float ZoomRatio = 1.2f;
        private float _zoom;

        #endregion

        #region Constructors

        public Camera()
        {
            Zoom = 1;
        }

        #endregion

        #region Properties

        public float Zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = Math.Max(MinZoom, value);
                _zoom = Math.Min(MaxZoom, value);
            }
        }

        #endregion
    }
}