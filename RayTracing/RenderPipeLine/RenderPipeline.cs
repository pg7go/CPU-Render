using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RayTracing
{
    abstract class RenderPipeline
    {
        public int maxDepth = 50;


        public int width;
        public int height;
        public int samples;
        protected Color[,] colors;
        protected static Random random = new Random();

        public RenderPipeline(int width, int height, int samples)
        {
            this.width = width;
            this.height = height;
            this.samples = samples;
            colors = new Color[width, height];
        }

        public abstract Color[,] RenderScene(Scene scene, Camera camera,Action<float> updateCallback);




    }
}
