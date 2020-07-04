using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RayTracing
{
    public abstract class HitableObject
    {
        public Material material;
        public Transform transform=new Transform();
        public abstract bool Hit(Ray r, float tMin, float tMax, out HitRecord rec);
    }


    public class Transform
    {
        public Vector3 position=Vector3.Zero;
        public Vector3 eulerRotation=Vector3.Zero;
    }
}
