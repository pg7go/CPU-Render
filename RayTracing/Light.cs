using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RayTracing
{
    public class Light
    {
        public Vector3 position;
        public Vector3 color;
        public float insensity;

        public Light(Vector3 position, Vector3 color,float insensity=1f)
        {
            this.position = position;
            this.color = color;
            this.insensity = insensity;
        }
    }
}
