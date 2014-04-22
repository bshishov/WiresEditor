
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WEditor.Utilities
{
    struct SpriteCoord
    {
        public float X1;
        public float Y1;
        public float X2;
        public float Y2;
    }

    class SpriteFont
    {
        public int GlTextureId { get; private set; }
        public float CharWidth { get; private set; }
        public float CharHeight { get; private set; }
        public float Kern { get; private set; }

        private Dictionary<char, SpriteCoord> _coords = new Dictionary<char, SpriteCoord>();

        public SpriteFont(string filename, float charWidth = 14, float charHeight = 14)
        {
            GlTextureId = DrawingUtilities.LoadTexture(filename, false).GlId;
            CharWidth = charWidth;
            CharHeight = charHeight;
            Kern = -6;

            for (var i = 0; i < 256; i++)
            {
                var ch = (char)i;
                var row = i / 16;
                var col = i % 16;

                var coord = new SpriteCoord()
                {
                    X1 = col / 16f,
                    X2 = (col + 1) / 16f,
                    Y1 = row / 16f,
                    Y2 = (row + 1) / 16f
                };
                _coords.Add(ch, coord);
            }
        }

        public SpriteFont(Font font)
        {
            CharWidth = font.Size * 1.2f;
            CharHeight = font.Size * 1.6f;
            Kern = -CharWidth / 4;
            var spritefont = new Bitmap((int)(CharWidth * 256), (int)CharHeight);
            using (var gfx = Graphics.FromImage(spritefont))
            {
                //gfx.Clear(ColorTranslator.FromHtml("#FF00FF"));
                gfx.Clear(Color.Transparent);
                gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                var brush = new SolidBrush(Color.White); 
                
                for (var i = 0; i < 256; i++)
                {
                    var ch = (char) i;
                    if (i >= 32 && i <= 128)
                    {
                        gfx.DrawString(
                            ch.ToString(), 
                            font, 
                            brush, 
                            i * CharWidth, 
                            0);
                    }

                    const float frac = 1 / 256f;
                    _coords.Add(ch, new SpriteCoord()
                    {
                        X1 = i * frac,
                        X2 = (i + 1) * frac,
                        Y1 = 0f,
                        Y2 = 1f,
                    });
                }
            }
            GlTextureId = DrawingUtilities.LoadTexture(spritefont, true).GlId;
        }

        public SpriteCoord Get(char ch)
        {
            return _coords[ch];
        }
    }
}
