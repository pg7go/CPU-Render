using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;


namespace RayTracing
{
    class EasyPipeLine: RenderPipeline
    {

        float progress = 0;

        public EasyPipeLine(int width, int height, int samples=0) : base(width, height, samples)
        {
            maxDepth = 5;
        }

        public override Color[,] RenderScene(Scene scene, Camera camera, Action<float> updateProcessCallback)
        {
            progress = 0;
            //Parallel.For(0, height, y =>
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float xLerp = (float)x  / width;
                    float yLerp = (float)y  / height;
                    Ray ray = camera.GetRay(xLerp, yLerp);
                    Vector3 colorValue = GetColorValue(ray, scene, 0);


                    colorValue = Vector3.Clamp(colorValue,Vector3.Zero, Vector3.One);
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
                if (depth < maxDepth && rec.material.Scatter(scene, ray, rec, out Attenuation attenuation, out Ray scattered))
                {
                    if (attenuation.attenuationType != AttenuationType.None&& attenuation.forceToDoAnotherPass)
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
