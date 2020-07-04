using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing
{
    class Render
    {

        public RenderPipeline renderPipeline;

        public Render(RenderPipeline renderPipeline)
        {
            this.renderPipeline = renderPipeline;
        }

        Bitmap lastRenderedMap;
        public Bitmap RenderScene(Scene scene, Camera camera)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            var colors = renderPipeline.RenderScene(scene, camera, Update);

            int width = colors.GetLength(0);
            int height = colors.GetLength(1);
            lastRenderedMap = new Bitmap(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    lastRenderedMap.SetPixel(x, y, colors[x, y]);
                }
            }

            stopwatch.Stop();
            CalculateRenderTime(stopwatch.ElapsedMilliseconds);
            return lastRenderedMap;
        }

        DateTime lastUpdateTime = DateTime.Now;
        void Update(float progress)
        {
            DateTime timeNow = DateTime.Now;

            
            if((timeNow-lastUpdateTime).TotalSeconds>0.2f|| progress==1)
            {
                lastUpdateTime = timeNow;
                Console.Clear();
                Console.WriteLine("Progress:");
                progress *= 100;
                float num = progress / 10;
                for (int i = 0; i < 10; i++)
                {
                    if (i >= num)
                        Console.Write("□");
                    else
                        Console.Write("■");
                }
                Console.WriteLine($"  {progress:n}%");
            }


        }

        void CalculateRenderTime(float ms)
        {
            Console.WriteLine("\n\n渲染结束");
            Console.WriteLine($"渲染分辨率：{renderPipeline.width}×{renderPipeline.height}");
            Console.WriteLine($"渲染耗时：{ms/1000}秒");
        }




        public string SaveRenderedMap(string fileName = "Render.png")
        {
            if (lastRenderedMap == null)
                return null;

            lastRenderedMap.Save(fileName, ImageFormat.Png);
            return $"{AppDomain.CurrentDomain.BaseDirectory}{fileName}";
        }



    }
}
