using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RayTracing
{
    public abstract class Material
    {
        public abstract bool Scatter(Scene scene,Ray ray, HitRecord hit, out Attenuation attenuation, out Ray scattered);

        public bool CastShadow(Scene scene, Light light,HitRecord hit)
        {
            Vector3 toLightDir = Vector3.Normalize( light.position- hit.position);
            if (scene.HitOne(new Ray(hit.position, toLightDir), out HitRecord rec))
                return true;
            return false;
        }

    }

    public abstract class TextruedMaterial:Material
    {
        Texture texture;
        public TextruedMaterial(Texture texture)
        {
            this.texture = texture;
        }

        public Vector3 GetTextureColor(Vector2 uv)
        {
            //uv = new Vector2(uv.Y, uv.X);
            if (texture == null)
                return Vector3.One;
            var color= texture.GetColor(uv);
            return new Vector3(color.R/255f, color.G/255f, color.B/255f);
        }

    }




}
