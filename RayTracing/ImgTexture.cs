using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Numerics;

namespace RayTracing
{
    class ImgTexture: Texture
    {
        Bitmap texture;
        Vector2 uvOffset=Vector2.Zero;
        Vector2 uvScale = Vector2.One;


        public ImgTexture(string fileName)
        {
            texture = new Bitmap(fileName);
        }
        public ImgTexture(string fileName, Vector2 uvOffset)
        {
            texture = new Bitmap(fileName);
            this.uvOffset = uvOffset;
        }


        public ImgTexture(string fileName, Vector2 uvOffset, Vector2 uvScale)
        {
            texture = new Bitmap(fileName);
            this.uvOffset = uvOffset;
            this.uvScale = uvScale;
        }

        public override Color GetColor(Vector2 uv)
        {
            uv=FormatUV(uv);
            uv *= uvScale;
            uv += uvOffset;
            uv = FormatUV(uv);

            int u = (int)Math.Floor(uv.X * (texture.Width - 1));
            int v = (int)Math.Floor(uv.Y * (texture.Height - 1));
            return texture.GetPixel( u, texture.Height-1-v);
        }

        Vector2 FormatUV(Vector2 uv)
        {
            while (uv.X > 1)
                uv.X -= 1;
            while (uv.X < 0)
                uv.X += 1;
            while (uv.Y > 1)
                uv.Y -= 1;
            while (uv.Y < 0)
                uv.Y += 1;
            return uv;
        }


    }
}
