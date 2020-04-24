using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RayTracing
{
    class LinearColor : Material
    {
        Vector3 skyColor= new Vector3(0.5f, 0.7f, 1f);
        public LinearColor()
        {

        }
        public LinearColor(Vector3 skyColor)
        {
            this.skyColor = skyColor;
        }


        public override bool Scatter(Scene scene, Ray ray, HitRecord hit, out Attenuation attenuation, out Ray scattered)
        {
            Vector3 unitDirection = Vector3.Normalize(ray.direction);
            float t = (0.5f * unitDirection.Y) + 1.0f;
            Vector3 colorValue = ((1.0f - t) * new Vector3(1f, 1f, 1f)) + (t * skyColor);
            
            attenuation = Attenuation.None(colorValue);
            scattered = null;
            return true;
        }
    }
}
