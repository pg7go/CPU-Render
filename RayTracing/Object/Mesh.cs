using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RayTracing
{
    class Mesh : HitableObject
    {
        List<Fragment> fragments = new List<Fragment>();

        public Mesh(List<Fragment> fragments) 
        {
            this.fragments = fragments;
        }

        /// <summary>
        /// 以Maya导出格式为准
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static Mesh CreateFromObj(string file,Material material)
        {
            var text=System.IO.File.ReadAllText(file);
            var lines=text.Split('\n');

            List<Fragment> fragments = new List<Fragment>();
            List<Vector3> pts = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();

            foreach (var line in lines)
            {
                var items = line.Split(' ');
                if(items.Length>0)
                {
                    switch (items[0])
                    {
                        case "v":
                            pts.Add(new Vector3(float.Parse(items[1]), float.Parse(items[2]), float.Parse(items[3])));
                            break;
                        case "f":
                            List<Vector3> fragPts = new List<Vector3>();
                            List<Vector3> fragNormals = new List<Vector3>();
                            List<Vector2> fragUvs = new List<Vector2>();
                            for (int i = 1; i < items.Length; i++)
                            {
                                var infos = items[i].Split('/');

                                fragPts.Add(pts[int.Parse(infos[0])-1]);
                                fragUvs.Add(uvs[int.Parse(infos[1])-1]);
                                fragNormals.Add(normals[int.Parse(infos[2])-1]);
                            }

                            fragments.Add(new Fragment(fragPts, material).SetUvs(fragUvs).SetNormals(fragNormals));
                            break;
                        case "vn":
                            normals.Add(new Vector3(float.Parse(items[1]), float.Parse(items[2]), float.Parse(items[3])));
                            break;
                        case "vt":
                            uvs.Add(new Vector2(float.Parse(items[1]), float.Parse(items[2])));
                            break;
                        default:
                            break;
                    }


                }
            }

            return new Mesh(fragments);


        }





        public override bool Hit(Ray ray, float tMin, float tMax, out HitRecord rec)
        {
            rec = null;
            bool hitAnything = false;
            float closestSoFar = tMax;

            foreach (var ob in fragments)
            {
                if (ob.Hit(ray, tMin, tMax, out HitRecord recTemp))
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
    }
}
