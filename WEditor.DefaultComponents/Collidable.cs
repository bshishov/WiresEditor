﻿#region

using System.ComponentModel;
using System.ComponentModel.Composition;
using WEditor.ComponentLibBase;

#endregion

namespace WEditor.DefaultComponents
{
    [Export(typeof (IGameComponent))]
    [ExportMetadata("Type", typeof (Collidable))]
    internal class Collidable : IGameComponent
    {
        #region Constructors

        public Collidable()
        {
            OffsetX = 0;
            OffsetY = 0;
            ScaleX = 1;
            ScaleY = 1;
        }

        #endregion

        #region Properties

        [Category("Offset")]
        [Description("Offset to object position along X axis relative to object size")]
        public float OffsetX { get; set; }

        [Category("Offset")]
        [Description("Offset to object position along Y axis relative to object size")]
        public float OffsetY { get; set; }

        [Category("Scale")]
        [Description("Scale along X axis relative to object size")]
        public float ScaleX { get; set; }

        [Category("Scale")]
        [Description("Scale along Y axis relative to object size")]
        public float ScaleY { get; set; }

        [Category("Main")]
        [Description("Is sensor means tha it will emit OnCollision message but it's not physically stops the other bodies")]
        public bool IsSensor { get; set; }

        #endregion

        #region Methods

        public override string ToString()
        {
            return "Collidable";
        }

        #endregion
    }
}