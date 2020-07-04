using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

namespace RayTracing
{
    class Program
    {
        

        static void Main(string[] args)
        {

            Scene scene = new Scene();

            var woodTex = new ImgTexture("wood.png");
            var woodMat = new Phong(woodTex, Vector3.One, 0.2f, 1);

            scene.hitableObjects.Add(Mesh.CreateFromObj("wood2.obj", woodMat));

            var planeTex = new ImgTexture("plane.png");
            var planeMat = new Phong(planeTex, Vector3.One,1, 10,0f);
            scene.hitableObjects.Add(Mesh.CreateFromObj("plane.obj", planeMat));

            var appleTex = new ImgTexture("apple.png");
            var phong = new Phong(appleTex, Vector3.One, 0.2f, 10);
            phong.attenuationType = AttenuationType.None;
            scene.hitableObjects.Add(Mesh.CreateFromObj("apple2.obj", phong));
            //scene.hitableObjects.Add(Mesh.CreateFromObj("apple1.obj", phong));
            //scene.hitableObjects.Add(Mesh.CreateFromObj("apple3.obj", phong));

            scene.clearMaterial = new HDRISkySphere(new ImgTexture("sky.jpg", new Vector2(-0.1f, -0.5f),new Vector2(1,-1)));



            scene.lights.Add(new Light(new Vector3(3, 2, 2), Vector3.One, 1));
            scene.ambientLightInsensity = 0.2f;
            int width = 400;
            int height = 200;

            //Vector3 lookFrom = new Vector3(1, 0.5f, 1.5f);
            Vector3 lookFrom = new Vector3(1, 1f, 1f);


            Vector3 lookat = new Vector3(0, 0, 0f);
            Camera camera = new Camera(lookFrom, lookat, Vector3.UnitY, 60, width / height, 0, (lookFrom - lookat).Length());
            camera.hardShadow = true;


            //(scene, camera) = Scene.CreateCornellBoxScene();

            //Render render = new Render(new RayTracingRenderPipeLine(width, height, 500,10));
            Render render = new Render(new EasyPipeLine(width, height, 0));


            render.RenderScene(scene, camera);
            ShowPic(render.SaveRenderedMap($"Render.png"));
        }





        public static void ShowPic(string path)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = path;
            startInfo.UseShellExecute = true;
            startInfo.CreateNoWindow = true;
            startInfo.Verb = string.Empty;
            Process.Start(startInfo);
        }





    }
}
