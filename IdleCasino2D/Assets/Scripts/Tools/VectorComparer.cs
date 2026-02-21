using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorComparer
{
    public static System.Numerics.Vector3 ToSystemVector3(this Vector3 v)
    {
        return new System.Numerics.Vector3(v.x, v.y, v.z);
    }

    public static Vector3 ToUnityVector3(this System.Numerics.Vector3 v)
    {
        return new Vector3(v.X, v.Y, v.Z);
    }
}
