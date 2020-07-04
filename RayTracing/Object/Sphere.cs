using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RayTracing
{
    public class Sphere: HitableObject
    {
        public float radius;
        static Random random=new Random();

        public static Vector3 RandomInUnitSphere()
        {
            Vector3 p;
            do
            {
                p = (new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble()))*2 - Vector3.One;
            } while (p.Length() >= 1);

            return p;
        }

        public Sphere(Vector3 center, float radius,Material material)
        {
            this.transform.position = center;
            this.radius = radius;
            base.material = material;
        }

        public override bool Hit(Ray ray,float tMin,float tMax, out HitRecord rec)
        {
            Vector3 oc = ray.from - transform.position;
            float a = Vector3.Dot(ray.direction, ray.direction);
            float b = Vector3.Dot(oc, ray.direction);
            float c = Vector3.Dot(oc, oc) - radius * radius;
            float discriminant = (b * b) - (a * c);
            if( discriminant > 0)
            {
                float temp= (-b - MathF.Sqrt(discriminant)) / a;
                if (temp >= tMax || temp <= tMin)
                {
                    temp = (-b + MathF.Sqrt(discriminant))  / a;
                    if (temp >= tMax || temp <= tMin)
                    {
                        rec = null;
                        return false;
                    }
                }

                rec = new HitRecord();
                rec.t = temp;
                rec.position = ray.GetPointAt(rec.t);
                //rec.normal = Vector3.Normalize((rec.position - center);
                rec.normal = (rec.position - transform.position) /radius;
                rec.material = material;
                rec.uv = GetUV(rec.position);
                return true;
            }


            rec = null;
            return false;
        }

        public Vector2 GetUV(Vector3 pos)
        {
            Vector3 dir = Vector3.Normalize(pos - transform.position);
            //旋转
            if(transform.eulerRotation!=Vector3.Zero)
                dir=Vector3.TransformNormal(dir, Matrix4x4.CreateFromYawPitchRoll(transform.eulerRotation.X, transform.eulerRotation.Y, transform.eulerRotation.Z));

            float phi = MathF.Atan2(dir.Z, dir.X);
            float theta = MathF.Acos(dir.Y);


            float u = 1 - (phi + MathF.PI) / (2 * MathF.PI);
            float v = (theta + MathF.PI / 2) / MathF.PI;
            return new Vector2(u, v);

            //if (phi < 0)
            //    phi += 2 * MathF.PI;
            //return new Vector2( 1 - theta / MathF.PI,phi / (2 / MathF.PI));
        }


    }




}
