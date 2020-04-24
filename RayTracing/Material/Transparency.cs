using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RayTracing
{
    class Transparency : TextruedMaterial
    {
        private Vector3 albedo;
        private float transparency;

        public Transparency(Vector3 albedo, float transparency, Texture texture = null) : base(texture)
        {
            this.albedo = albedo;
            this.transparency = transparency;
        }

        public override bool Scatter(Scene scene, Ray ray, HitRecord hit, out Attenuation attenuation, out Ray scattered)
        {
            scattered = new Ray(hit.position, ray.direction);

            var textureColor = GetTextureColor(hit.uv);
            attenuation = Attenuation.Add(albedo * textureColor, 1-transparency, albedo * textureColor);
            attenuation.forceToDoAnotherPass = true;
            return true;
        }
    }
}
