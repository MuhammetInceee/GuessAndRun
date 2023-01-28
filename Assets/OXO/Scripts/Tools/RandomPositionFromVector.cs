using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomPositionFromVector
{
    public static Vector3 FindClosePosition(Vector3 position, float radius)
    {
        Vector3 random = position + Random.onUnitSphere * radius;
        return random;
    }
}
