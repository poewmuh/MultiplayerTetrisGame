using System.Collections.Generic;
using UnityEngine;

namespace Tetris.Tools
{
    public static class ExtensionMethods
    {
        public static Vector3 OnlyXZ(this Vector3 v3)
        {
            v3.y = 0f;
            return v3;
        }
        
        public static float Sqr(this float f)
        {
            return f * f;
        }

        public static bool ApproxZero(this float f)
        {
            return Mathf.Approximately(0, f);
        }
        
        public static float Abs(this float f)
        {
            return Mathf.Abs(f);
        }

        public static bool Approx(this float f, float f2)
        {
            return Mathf.Approximately(f, f2);
        }
        
        public static bool TryAdd<T> (this List<T> list, T value)
        {
            if (list.Contains(value))
                return false;
            
            list.Add(value);
            return true;
        }
        
        public static void ShuffleList<T>(IList<T> list)
        {
            System.Random rand = new System.Random();
            for (int i = list.Count - 1; i >= 1; i--)
            {
                int j = rand.Next(i + 1);
                (list[j], list[i]) = (list[i], list[j]);
            }
        }
    }
}