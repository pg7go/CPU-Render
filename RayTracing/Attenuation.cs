using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RayTracing
{
    public class Attenuation
    {
        public Vector3 color;
        public Vector3 attenuationColor;
        public float blendPercent=0.5f;
        /// <summary>
        /// 仅仅在easyPipeline起作用
        /// </summary>
        public bool forceToDoAnotherPass = false;
        public AttenuationType attenuationType= AttenuationType.Multiply;
        public Vector3 BlendWith(Vector3 other)
        {
            switch (attenuationType)
            {
                case AttenuationType.Multiply:
                    return color *  other;

                case AttenuationType.Add:
                    return color * blendPercent + (1 - blendPercent) * other* attenuationColor;

                case AttenuationType.None:
                    return color;

                default:
                    return color;
            }

        }


        private Attenuation() { }

        public static Attenuation Multiply(Vector3 color)
        {
            var attenuation = new Attenuation();
            attenuation.attenuationType = AttenuationType.Multiply;
            attenuation.color = color;
            return attenuation;
        }


        public static Attenuation Add(Vector3 color,float percent, Vector3 attenuationColor)
        {
            var attenuation = new Attenuation();
            attenuation.attenuationType = AttenuationType.Add;
            attenuation.color = color;
            attenuation.blendPercent = percent;
            attenuation.attenuationColor = attenuationColor;
            return attenuation;
        }

        public static Attenuation None(Vector3 color)
        {
            var attenuation = new Attenuation();
            attenuation.attenuationType = AttenuationType.None;
            attenuation.color = color;
            return attenuation;
        }

    }

    public enum AttenuationType
    {
        Multiply,
        Add,
        None
    }
}
