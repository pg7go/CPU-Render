using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RayTracing
{
    public class Lit : TextruedMaterial
    {
        private Vector3 albedo;

        public Lit( Vector3 albedo, Texture texture=null):base(texture)
        {
            this.albedo = albedo;
        }

        public override bool Scatter(Scene scene, Ray ray,HitRecord hit, out Attenuation attenuation, out Ray scattered)
        {
            //skyMat
            if(hit==null)
            {
                attenuation = Attenuation.None(albedo);
                scattered = null;
                return true;
            }

            scattered = new Ray(hit.position, ray.direction);
            var textureColor = GetTextureColor(hit.uv);
            //attenuation = Attenuation.None(new Vector3(hit.uv.X, hit.uv.Y,0));
            attenuation = Attenuation.None(albedo * textureColor);

            return true;
        }
    }
}
