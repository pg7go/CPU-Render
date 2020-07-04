using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace RayTracing
{
    public abstract class Texture
    {
        public abstract Color GetColor(Vector2 uv);

    }
}
