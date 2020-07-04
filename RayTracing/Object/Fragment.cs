using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RayTracing
{
    class Fragment : HitableObject
    {
        public List<Vector3> points = new List<Vector3>();
        public List<Vector3> normals = new List<Vector3>();
        public List<Vector2> uvs = new List<Vector2>();

        public bool cullBack = false;

        /// <summary>
        /// 朝向遵循左手定则
        /// </summary>
        /// <param name="points"></param>
        /// <param name="material"></param>
        public Fragment(List<Vector3> points, Material material,bool cullBack=false,bool flipNormal=false)
        {
            if (points.Count < 3)
                throw new Exception("多边形面片至少需要3个点");
            if (flipNormal)
                points.Reverse();
            this.points = points;
            this.material = material;
            this.cullBack = cullBack;
        }

        public Fragment SetUvs(List<Vector2> uvs)
        {
            this.uvs = uvs;
            return this;
        }
        public Fragment SetNormals(List<Vector3> normals)
        {
            this.normals = normals;
            return this;
        }


        Vector3 GetAvgNormal(Vector3 point)
        {
            //return uvs[0];
            List<float> distances = new List<float>();
            float allDis = 0;
            for (int i = 0; i < points.Count; i++)
            {
                var pt = points[i];
                var dis = Vector3.Distance(point, pt);
                allDis += dis;
                distances.Add(dis);
                if (dis == 0)
                    return normals[i];
            }

            Vector3 n = Vector3.Zero;
            int index = 0;
            foreach (var item in normals)
            {
                n += item * (allDis - distances[index++]) / allDis / (points.Count - 1);
            }
            return n;

        }
        Vector2 GetAvgUV(Vector3 point)
        {
            //return uvs[0];
            List<float> distances = new List<float>();
            float allDis = 0;
            for (int i = 0; i < points.Count; i++)
            {
                var pt = points[i];
                var dis =  Vector3.Distance(point, pt);
                allDis += dis;
                distances.Add(dis);
                if (dis == 0)
                    return uvs[i];
            }

            Vector2 uv = Vector2.Zero;
            int index = 0;
            foreach (var item in uvs)
            {
                uv += item * (allDis-distances[index++]) / allDis/(points.Count-1);
                //uv += item * distances[index++] / allDis;
            }
            return uv;

        }

        Vector3 GetAvgNormalInTriangle(Vector3 point)
        {
            //return uvs[0];

            List<float> distances = new List<float>();
            float allDis = 0;

            points.Add(points[0]);
            points.Add(points[1]);
            points.Remove(points[0]);//1 2 0 1
            for (int i = 0; i < points.Count - 1; i++)
            {
                var dis = GetArea(
                    Vector3.Distance(points[i], points[i + 1]),
                    Vector3.Distance(point, points[i + 1]),
                    Vector3.Distance(points[i], point)
                    );
                allDis += dis;
                distances.Add(dis);
            }
            points.Add(points[1]);
            points.Remove(points[0]);
            points.Remove(points[0]);

            Vector3 n = Vector3.Zero;
            int index = 0;
            foreach (var normal in normals)
            {
                n += normal * distances[index++] / allDis;
            }
            return n;

        }



        Vector2 GetAvgUVInTriangle(Vector3 point)
        {
            //return uvs[0];
            
            List<float> distances = new List<float>();
            float allDis = 0;

            points.Add(points[0]);
            points.Add(points[1]);
            points.Remove(points[0]);//1 2 0 1
            for (int i = 0; i < points.Count-1; i++)
            {
                var dis = GetArea(
                    Vector3.Distance(points[i], points[i + 1]),
                    Vector3.Distance(point, points[i + 1]),
                    Vector3.Distance(points[i], point)
                    );
                allDis += dis;
                distances.Add(dis);
            }
            points.Add(points[1]);
            points.Remove(points[0]);
            points.Remove(points[0]);

            Vector2 uv = Vector2.Zero;
            int index = 0;
            foreach (var item in uvs)
            {
                uv += item * distances[index++] / allDis;
            }
            return uv;

        }

        static float GetArea(float a,float b,float c)
        {
            //一条直线
            if (a + b - c < 1e-5 || b + c - a < 1e-5 || c + a - b < 1e-5)
                return 0;
            var p = (a + b + c) / 2;
            return MathF.Sqrt(p * (p - a) * (p - b) * (p - c));
        }


        public override bool Hit(Ray r, float tMin, float tMax, out HitRecord rec)
        {
            //Plane plane = Plane.CreateFromVertices(points[0], points[1], points[2]);
            var normal = Vector3.Normalize(Vector3.Cross((points[0]-points[1]),(points[1] - points[2])));
            r.direction = Vector3.Normalize(r.direction);
            rec = new HitRecord();
            //面片背面剔除
            var cos = Vector3.Dot(r.direction, normal);
            if (cos == 0)
            {
                return false;
            }
            if (cullBack&& cos < 0)
                return false;


            //计算t（距离）
            rec.t = Vector3.Dot(points[0] - r.from, normal) / Vector3.Dot(r.direction, normal);
            //rec.t = (Vector3.Dot(normal,points[0]) - Vector3.Dot(normal, r.from) )/ Vector3.Dot(normal,r.direction );

            if (float.IsNaN(rec.t)||rec.t < tMin||rec.t>tMax)
                return false;

            //计算p（平面交点）
            var p = r.from+ rec.t * r.direction;

            //光栅剔除
            points.Add(points[0]);
            Vector3 firstTowards = Vector3.Zero;

            for (int i = 0; i < points.Count-1; i++)
            {
                var oa = Vector3.Normalize( points[i] - points[i + 1]);
                var op = Vector3.Normalize(points[i] - p);
                var towards = Vector3.Normalize(Vector3.Cross(oa, op));
                if (firstTowards == Vector3.Zero)
                    firstTowards = towards;
                else if(Vector3.Dot(firstTowards,towards)<0.999999)
                {
                    points.RemoveAt(points.Count - 1);
                    return false;
                }
            }
            points.RemoveAt(points.Count - 1);
            rec.position = p;

            //是否有normal记录
            if (normals.Count == points.Count)
            {
                if (uvs.Count == 3)
                    rec.normal = GetAvgNormalInTriangle(p);
                else
                    rec.normal = GetAvgNormal(p);
            }else
                rec.normal = normal;



            //是否有uv记录
            if (uvs.Count == points.Count)
            {
                if(uvs.Count==3)
                    rec.uv = GetAvgUVInTriangle(p);
                else
                    rec.uv = GetAvgUV(p);
            }
                

            rec.material = material;


            return true;
        }

    }

}
