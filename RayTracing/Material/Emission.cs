using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RayTracing
{
    class Emission : TextruedMaterial
    {
        private Vector3 color;
        private float intensity = 1f;

        public Emission(Texture texture, Vector3 color, float intensity=1) : base(texture)
        {
            this.color = color;
            this.intensity = intensity;
        }

        public override bool Scatter(Scene scene, Ray ray, HitRecord hit, out Attenuation attenuation, out Ray scattered)
        {
            scattered = null;

            var textureColor = GetTextureColor(hit.uv);
            attenuation = Attenuation.None(color * textureColor * intensity);

            return true;

        }
    }
}
