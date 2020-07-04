using System;
using System.Collections.Generic;
using System.Numerics;

namespace RayTracing
{
    public class Scene
    {
        public Vector3 ambientLightColor { protected get;  set; } = Vector3.One;
        public float ambientLightInsensity { protected get; set; } = 0;
        public Vector3 ambientColor { get { return ambientLightColor * ambientLightInsensity; } }

        public Material clearMaterial = new LinearColor();

        public List<HitableObject> hitableObjects = new List<HitableObject>();
        public List<Light> lights = new List<Light>();
        public bool Hit(Ray ray, out HitRecord rec, float tMin = 0.001f, float tMax=10000)
        {
            rec = null;
            bool hitAnything = false;
            float closestSoFar = tMax;

            foreach (var ob in hitableObjects)
            {
                if ( ob.Hit(ray, tMin, tMax, out HitRecord recTemp))
                {
                    hitAnything = true;
                    if (closestSoFar > recTemp.t)
                    {
                        closestSoFar = recTemp.t;
                        rec = recTemp;
                    }
                }
            }
            return hitAnything;
        }

        public bool HitOne(Ray ray, out HitRecord rec, float tMin = 0.001f, float tMax = 10000)
        {
            foreach (var ob in hitableObjects)
            {
                if ( ob.Hit(ray, tMin, tMax, out HitRecord recTemp))
                {
                    rec = recTemp;
                    return true;
                }
            }
            rec = null;
            return false;
        }



        static Random random = new Random();
        static float randF { get { return (float)random.NextDouble(); } }


        public static Scene CreateRandomScene(int objectNumber = 11)
        {
            Scene scene = new Scene();
            for (int a = -objectNumber; a < objectNumber; a++)
            {
                for (int b = -objectNumber; b < objectNumber; b++)
                {
                    float choose_mat = randF;
                    Vector3 center = new Vector3(a + 0.9f * randF, 0.2f, b + 0.9f * randF);
                    if ((center - new Vector3(4f, 0.2f, 0)).Length() > 0.9f)
                    {
                        if (choose_mat < 0.8f) // Diffuse
                        {
                            scene.hitableObjects.Add(new Sphere(center, 0.2f, new Lambertian(new Vector3(randF * randF, randF * randF, randF * randF))));
                        }
                        else if (choose_mat < 0.95f) // Metal
                        {
                            scene.hitableObjects.Add(new Sphere(center, 0.2f, new Metal(new Vector3(0.5f * (1f + randF), 0.5f * (1f + randF), 0.5f * (1 + randF)), 0.5f * randF)));
                        }
                        else // glass
                        {
                            scene.hitableObjects.Add(new Sphere(center, 0.2f, new Dielectric(1.5f)));
                        }
                    }
                }
            }

            scene.hitableObjects.Add(new Sphere(new Vector3(0, -1000, 0), 1000, new Lambertian(new Vector3(0.5f, 0.5f, 0.5f))));

            scene.hitableObjects.Add(new Sphere(new Vector3(0, 1, 0), 1f, new Dielectric(1.5f)));
            scene.hitableObjects.Add(new Sphere(new Vector3(-4, 1, 0), 1f, new Lambertian(new Vector3(0.4f, 0.2f, 0.1f))));
            scene.hitableObjects.Add(new Sphere(new Vector3(4f, 1f, 0f), 1f, new Metal(new Vector3(0.7f, 0.6f, 0.5f), 0f)));


            return scene;

        }


        public static (Scene,Camera) CreateCornellBoxScene(float fov=1)
        {
            Scene scene = new Scene();
            scene.clearMaterial = new Lit(Vector3.One);


            //var wallMat = new Lambertian(Vector3.One);
            var wallMat = new Phong(null, Vector3.One, 0.4f, 10);
            wallMat.attenuationType = AttenuationType.Multiply;
            var leftMat = new Phong(null, Vector3.UnitY, 0.5f, 10);
            leftMat.attenuationType = AttenuationType.Multiply;
            var rightMat = new Phong(null, Vector3.UnitX, 0.5f, 10);
            rightMat.attenuationType = AttenuationType.Multiply;

            //下
            scene.hitableObjects.Add(new Fragment(new List<Vector3>() 
            { new Vector3(10, -10, 10), new Vector3(-10, -10, 10), new Vector3(-10, -10, -10), new Vector3(10, -10, -10) }, wallMat));

            scene.hitableObjects.Add(new Fragment(new List<Vector3>()
            { new Vector3(10, -10, -10), new Vector3(-10, -10, -10), new Vector3(-10, 10, -10), new Vector3(10, 10, -10) }, wallMat));

            //上
            scene.hitableObjects.Add(new Fragment(new List<Vector3>()
            { new Vector3(10, 10, 10), new Vector3(-10, 10, 10), new Vector3(-10, 10, -10), new Vector3(10, 10, -10) }, wallMat));

            scene.hitableObjects.Add(new Fragment(new List<Vector3>()
            { new Vector3(-10, -10, 10), new Vector3(-10, -10, -10), new Vector3(-10, 10, -10), new Vector3(-10, 10, 10) }, leftMat));

            scene.hitableObjects.Add(new Fragment(new List<Vector3>()
            { new Vector3(10, -10, 10), new Vector3(10, -10, -10), new Vector3(10, 10, -10), new Vector3(10, 10, 10) }, rightMat));

            scene.hitableObjects.Add(new Fragment(new List<Vector3>()
            { new Vector3(5f, 9.9f, 0f), new Vector3(-5f, 9.9f, 0f), new Vector3(-5f, 9.9f, -3f), new Vector3(5f, 9.9f, -3f) }, 
                new Emission(null,Vector3.One,15)));

            scene.hitableObjects.Add(new Fragment(new List<Vector3>()
            { new Vector3(10, -10, 10f), new Vector3(-10, -10, 10f), new Vector3(-10, 10, 10f), new Vector3(10, 10, 10f) }, new Lit(Vector3.One),true,true));



            //背后遮板
            //scene.hitableObjects.Add(new Fragment(new List<Vector3>()
            //{ new Vector3(1000, -1000, 40), new Vector3(-1000, -1000, 40), new Vector3(-1000, 1000, 40), new Vector3(1000, 1000, 40) },
            //    new Lit(Vector3.One)
            //));



            Vector3 lookFrom = new Vector3(0, 0, 37f);
            Vector3 lookat = new Vector3(0, 0, 0f);
            Camera camera = new Camera(lookFrom, lookat, Vector3.UnitY, 40, fov, 0, (lookFrom - lookat).Length());

            //光照
            //scene.lights.Add(new Light(new Vector3(0,9.8f,-1.5f), Vector3.One, 1));
            scene.ambientLightInsensity = 0.2f;


            return (scene, camera);

        }


    }
}
