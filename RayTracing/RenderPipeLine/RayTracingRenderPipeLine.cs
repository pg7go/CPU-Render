using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing
{
    class RayTracingRenderPipeLine : RenderPipeline
    {
       float progress = 0;

        public RayTracingRenderPipeLine(int width, int height, int samples,int maxDepth=50) : base(width, height, samples)
        {
            this.maxDepth = maxDepth;
        }

        public override Color[,] RenderScene(Scene scene, Camera camera, Action<float> updateProcessCallback)
        {
            progress = 0;
            //Parallel.For(0, height, y =>
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {

                    Vector3 colorValue = Vector3.Zero;
                    for (int i = 0; i < samples; i++)
                    {
                        float xLerp = (float)(x + random.NextDouble()) / width;
                        float yLerp = (float)(y + random.NextDouble()) / height;
                        Ray ray = camera.GetRay(xLerp, yLerp);
                        colorValue += GetColorValue(ray, scene, 0);
                    }

                    colorValue /= samples;
                    colorValue = Vector3.Clamp(colorValue, Vector3.Zero, Vector3.One);
                    // Gamma 2
                    colorValue.X = MathF.Sqrt(colorValue.X);
                    colorValue.Y = MathF.Sqrt(colorValue.Y);
                    colorValue.Z = MathF.Sqrt(colorValue.Z);

                    colors[x, height - y - 1] = Color.FromArgb((int)(255 * colorValue.X), (int)(255 * colorValue.Y), (int)(255 * colorValue.Z));


                    progress += (float)1 / height / width;
                    if (progress > 1)
                        progress = 1;
                    updateProcessCallback?.Invoke(progress);


                }



            }
            updateProcessCallback?.Invoke(1);
            return colors;
        }




        public Vector3 GetColorValue(Ray ray, Scene scene, int depth)
        {

            if (scene.Hit(ray, out HitRecord rec))
            {
                if (depth < maxDepth && rec.material.Scatter(scene,ray, rec, out Attenuation attenuation, out Ray scattered))
                {
                    if (attenuation.attenuationType != AttenuationType.None)
                        return attenuation.BlendWith(GetColorValue(scattered, scene, depth + 1));
                    else
                        return attenuation.color;
                }
                else
                {
                    return scene.ambientColor;//Vector3.Zero;
                }
            }

            scene.clearMaterial.Scatter(scene, ray, rec, out Attenuation clearAttenuation, out Ray nextRay);
            return clearAttenuation.color;
        }




    }
}
