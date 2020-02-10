using UnityEngine;

namespace NissenCode
{
    public class Helper
    {
        public struct Bound
        {
            public Vector3 min;
            public Vector3 max;
            public Vector3 center
            {
                get { return (max + min) / 2; }
            }
            public Vector3 size
            {
                get {
                    Vector3 size = (min -max);
                    size.x = Mathf.Abs(size.x);
                    size.y = Mathf.Abs(size.y);
                    size.z = Mathf.Abs(size.z);
                    return size; }
            }
        }
        public static bool RayInBound(Ray ray, Bounds bound, float distance = Mathf.Infinity)
        {
            float dist;
            return (bound.IntersectRay(ray, out dist) && dist <= distance);
        }
    }
}
