using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RayTracing
{
    class HDRISkySphere : TextruedMaterial
    {
        public Vector3 eulerRotation;

        public HDRISkySphere(Texture texture) : base(texture)
        {
        }

        public HDRISkySphere(Texture texture,Vector3 eulerRotation) : base(texture)
        {
            this.eulerRotation = eulerRotation;
        }

        public override bool Scatter(Scene scene, Ray ray, HitRecord hit, out Attenuation attenuation, out Ray scattered)
        {
            Vector3 dir = Vector3.Normalize(ray.direction);
            attenuation = Attenuation.None(GetTextureColor(GetUV(dir)));
            scattered = null;
            return true;
        }



        public Vector2 GetUV(Vector3 dir)
        {

            //旋转
            if (eulerRotation != Vector3.Zero)
                dir = Vector3.TransformNormal(dir, Matrix4x4.CreateFromYawPitchRoll(eulerRotation.X,eulerRotation.Y, eulerRotation.Z));

            float phi = MathF.Atan2(dir.Z, dir.X);
            float theta = MathF.Acos(dir.Y);


            float u = 1 - (phi + MathF.PI) / (2 * MathF.PI);
            float v = (theta + MathF.PI / 2) / MathF.PI;
            return new Vector2(u, v);

        }


    }
}
