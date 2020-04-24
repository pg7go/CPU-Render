using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RayTracing
{
    class Phong : TextruedMaterial
    {
        private Vector3 albedo;
        float specularIndex;
        float glossIndex;
        //镜面反射模糊
        float fuzz;
        //反射程度
        float blendPercent;
        public AttenuationType attenuationType = AttenuationType.Add;

        public Phong(Texture texture,Vector3 albedo, float specularIndex, float glossIndex, float fuzz=1,float blendPercent=0.7f) :base(texture)
        {
            this.albedo = albedo;
            this.specularIndex = specularIndex < 1 ? specularIndex : 1; ;
            this.glossIndex = glossIndex;
            this.fuzz = fuzz;
            this.blendPercent = blendPercent;
        }

        public override bool Scatter(Scene scene, Ray ray, HitRecord hit, out Attenuation attenuation, out Ray scattered)
        {
            var direction = Vector3.Normalize(ray.direction);
            Vector3 reflected = Vector3.Reflect(direction, hit.normal);
            scattered = new Ray(hit.position, reflected + fuzz * Sphere.RandomInUnitSphere());

            Vector3 diffuseColor = Vector3.Zero;
            Vector3 SpecularColor = Vector3.Zero;
            foreach (var light in scene.lights)
            {
                if (CastShadow(scene, light, hit))
                    continue;

                Vector3 lightDir = -Vector3.Normalize(hit.position - light.position);
                Vector3 reflectDir = Vector3.Reflect(-lightDir, hit.normal);

                 diffuseColor +=light.color * light.insensity * Vector3.Dot(hit.normal, lightDir);
                 SpecularColor+= light.color * light.insensity * specularIndex*
                    MathF.Pow(Math.Clamp(Vector3.Dot(reflectDir, -direction),0,1),glossIndex);
            }

            //环境光 + 光源
            var textureColor = GetTextureColor(hit.uv);
            var allColor= (scene.ambientColor + diffuseColor)* albedo * textureColor + SpecularColor; 
            attenuation = Attenuation.Add(allColor, blendPercent, albedo * textureColor);
            attenuation.attenuationType = attenuationType;

            //兼容easyPipeline
            if (fuzz == 0 && specularIndex == 1)
                attenuation.forceToDoAnotherPass = true;

            return true;//(Vector3.Dot(scattered.direction, hit.normal) > 0);
        }
    }
}
