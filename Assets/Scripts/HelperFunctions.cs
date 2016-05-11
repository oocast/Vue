using UnityEngine;
using System.Collections;

public static class HelperFunctions
{
    static readonly float enemyCenterHeight = 0.5f;

    public static float Distance2D(this Vector3 point1, Vector3 point2, int ignoreAxis = 1)
    {
        float result = 0.0f;
        float xDiff = point1.x - point2.x;
        float yDiff = point1.y - point2.y;
        float zDiff = point1.z - point2.z;
        switch(ignoreAxis)
        {
            case 0:
                result = Mathf.Sqrt(yDiff * yDiff + zDiff * zDiff);
                break;
            case 1:
                result = Mathf.Sqrt(xDiff * xDiff + zDiff * zDiff);
                break;
            case 2:
                result = Mathf.Sqrt(yDiff * yDiff + xDiff * xDiff);
                break;
            default:
                break;
        }
        return result;
    }

    public static float SquareDistance2D(this Vector3 point1, Vector3 point2, int ignoreAxis = 1)
    {
        float result = 0.0f;
        Vector3 vDiff = point1 - point2;
        switch (ignoreAxis)
        {
            case 0:
                vDiff.x = 0f;
                result = vDiff.sqrMagnitude;
                break;
            case 1:
                vDiff.y = 0f;
                result = vDiff.sqrMagnitude;
                break;
            case 2:
                vDiff.z = 0f;
                result = vDiff.sqrMagnitude;
                break;
            default:
                break;
        }
        return result;
    }

    public static Vector3 Direction2D(this Vector3 from, Vector3 to, int ignoreAxis = 1)
    {
        Vector3 result = to - from;
        switch (ignoreAxis)
        {
            case 0:
                result.x = 0f;
                break;
            case 1:
                result.y = 0f;
                break;
            case 2:
                result.z = 0f;
                break;
            default:
                break;
        }
        return result;
    }

}
