#region

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using Models.Components;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Color = System.Windows.Media.Color;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

#endregion

namespace WEditor.Utilities
{
    internal static class DrawingUtilities
    {
        #region Methods

        public static void DrawTriangle(PointF center, float rad, Color color)
        {
            GL.Begin(BeginMode.Triangles);
            GL.Color4(color.ScR, color.ScG, color.ScB, color.ScA);

            for (double i = 0; i <= 2*Math.PI; i += 2*Math.PI/3)
            {
                GL.Vertex3(center.X + Math.Cos(i + Math.PI/2)*rad,
                    center.Y + Math.Sin(i + Math.PI/2)*rad,
                    0);
            }
            GL.End();
        }

        public static void DrawRect(Rectangle rect, Color color)
        {
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(color.ScR, color.ScG, color.ScB, color.ScA);
            GL.Vertex3(rect.Left, rect.Top, 0);
            GL.Vertex3(rect.Right, rect.Top, 0);
            GL.Vertex3(rect.Right, rect.Bottom, 0);
            GL.Vertex3(rect.Left, rect.Bottom, 0);
            GL.End();
        }

        public static void DrawRect(float l, float r, float t, float b, Color color, PolygonMode mode)
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, mode);
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(color.ScR, color.ScG, color.ScB, color.ScA);
            GL.Vertex3(l, t, 0);
            GL.Vertex3(r, t, 0);
            GL.Vertex3(r, b, 0);
            GL.Vertex3(l, b, 0);
            GL.End();
            GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
        }

        public static void DrawLine(PointF a, PointF b, Color color)
        {
            GL.Begin(BeginMode.Lines);
            GL.Color4(color.ScR, color.ScG, color.ScB, color.ScA);
            GL.Vertex3(a.X, a.Y, 0);
            GL.Vertex3(b.X, b.Y, 0);
            GL.End();
        }

        public static TextureInfo LoadTexture(string filename, bool generateMipMaps = true)
        {
            if (String.IsNullOrEmpty(filename))
                throw new ArgumentException(filename);

            if (!File.Exists(filename))
                throw new FileNotFoundException(filename);

            return LoadTexture(new Bitmap(filename), generateMipMaps);
        }

        public static TextureInfo LoadTexture(Bitmap bmp, bool generateMipMaps = true)
        {
            var id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmpData.Width, bmpData.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);

            bmp.UnlockBits(bmpData);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            if (generateMipMaps)
            {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.Ext.GenerateTextureMipmap(id, TextureTarget.Texture2D);
            }
            else
            {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            }

            GL.BindTexture(TextureTarget.Texture2D, 0);
            return new TextureInfo(){GlId = id,Height = bmp.Height,Width = bmp.Width};
        }

        private static void InnerDraw(float x, float y, float w, float h, float scaleX, float scaleY, float rotation)
        {
            var p1 = new PointF(0, 0);
            var p2 = new PointF(scaleX, 0);
            var p3 = new PointF(scaleX, scaleY);
            var p4 = new PointF(0, scaleY);
            
            GL.TexCoord2(p1.X, p1.Y);
            GL.Vertex2(x, y);

            GL.TexCoord2(p4.X, p4.Y);
            GL.Vertex2(x, y + h);
            
            GL.TexCoord2(p3.X, p3.Y);
            GL.Vertex2(x + w, y + h);
            
            GL.TexCoord2(p2.X, p2.Y);
            GL.Vertex2(x + w, y);
        }

        public static void DrawTexture(
            PointF pos, 
            TextureInfo info, 
            float width, 
            float height,
            float rotation, 
            Color color)
        {
            GL.MatrixMode(MatrixMode.Texture);
            GL.LoadIdentity();
            var m = Matrix4.CreateFromAxisAngle(new Vector3(0, 0, 1), rotation);
            GL.LoadMatrix(ref m);

            GL.BindTexture(TextureTarget.Texture2D, info.GlId);
            
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(color.ScR, color.ScG, color.ScB, color.ScA);

            InnerDraw(pos.X, pos.Y, width, height, 1, 1, rotation);

            GL.End();
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public static void DrawTiledTexture(
            PointF pos, 
            TextureInfo info, 
            float width, 
            float height, 
            float scaleX, 
            float scaleY,
            float rotation, 
            Color color)
        {
            GL.MatrixMode(MatrixMode.Texture);
            GL.LoadIdentity();
            var m = Matrix4.CreateFromAxisAngle(new Vector3(0, 0, 1), rotation);
            GL.LoadMatrix(ref m);

            GL.BindTexture(TextureTarget.Texture2D, info.GlId);
            
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(color.ScR, color.ScG, color.ScB, color.ScA);

            InnerDraw(pos.X, pos.Y, width, height, scaleX, scaleY, rotation);

            GL.End();
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public static void DrawString(SpriteFont font, PointF pos, float scale, string text, Color color)
        {
            GL.BindTexture(TextureTarget.Texture2D, font.GlTextureId);
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(color.ScR, color.ScG, color.ScB, color.ScA);

            for (var i = 0; i < text.Length; i++)
            {
                var ch = text[i];
                var coords = font.Get(ch);

                //top left
                GL.TexCoord2(coords.X1, coords.Y1);
                GL.Vertex2(pos.X + (i * font.CharWidth + i * font.Kern) * scale, pos.Y);

                //top right
                GL.TexCoord2(coords.X2, coords.Y1);
                GL.Vertex2(pos.X + ((i + 1) * font.CharWidth + i * font.Kern) * scale, pos.Y);

                //bottom right
                GL.TexCoord2(coords.X2, coords.Y2);
                GL.Vertex2(pos.X + ((i + 1) * font.CharWidth + i * font.Kern) * scale, pos.Y + font.CharHeight * scale);

                //bottom left
                GL.TexCoord2(coords.X1, coords.Y2);
                GL.Vertex2(pos.X + (i * font.CharWidth + i * font.Kern) * scale, pos.Y + font.CharHeight * scale); 
            }

            GL.End();

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        #endregion
    }
}