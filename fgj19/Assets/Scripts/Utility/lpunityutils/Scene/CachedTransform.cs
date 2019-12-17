using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LPUnityUtils
{

    public class CachedTransform
    {
        public CachedTransform(Transform transform)
        {
            Position = transform.position;
            Rotation = transform.rotation;
        }

        public Vector3 Position;
        public Quaternion Rotation;
    }

    public static class TransformExtension
    {
        public static void Set(this Transform transform, CachedTransform cachedTransform)
        {
            transform.SetPositionAndRotation(cachedTransform.Position, cachedTransform.Rotation);
        }
    }

}
