using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public class Utility
    {
        public static System.Random random = new System.Random();

        /// <summary>
        /// Baraja de elementos de un array dinámico
        /// </summary>
        public static void KnuthShuffle<T>(List<T> collection)
        {
            for (int i = 0; i < collection.Count - 1; i++)
            {
                var j = random.Next(i, collection.Count);
                if (i != j)
                {
                    var temp = collection[j];
                    collection[j] = collection[i];
                    collection[i] = temp;
                }
            }
        }

        /// <summary>
        ///  Dibuja una flecha gizmo con dirección (en vez de solo una linea)
        /// </summary>
        public static void GizmoArrow(Vector3 from, Vector3 to, float scale = 0.25f, float gap = 0.15f)
        {
            var dir = to - from;
            to -= dir.normalized * gap;
            var offset = Vector3.Cross(dir.normalized, Vector3.up) * scale;
            var arrowLeft = to - dir.normalized * scale + offset;
            var arrowRight = to - dir.normalized * scale - offset;

            Gizmos.DrawLine(from, to);
            Gizmos.DrawLine(to, arrowLeft);
            Gizmos.DrawLine(to, arrowRight);
        }
    }
}
