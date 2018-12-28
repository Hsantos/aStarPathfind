using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heuristic {

    /// <summary>
    /// Calculates the Manhattan distance between the two points.
    /// </summary>
    /// <param name="x1">The first x coordinate.</param>
    /// <param name="x2">The second x coordinate.</param>
    /// <param name="y1">The first y coordinate.</param>
    /// <param name="y2">The second y coordinate.</param>
    /// <param name="z1">The first z coordinate.</param>
    /// <param name="z2">The second z coordinate.</param>
    /// <returns>The Manhattan distance between (x1, y1, z1) and (x2, y2, z2)</returns>
    public float CalculateManhattanDistance(Vector3 node , Vector3 target)
    {
        float x1 = node.x;
        float x2 = target.x;
        float y1 = node.y;
        float y2 = target.y;
        float z1 = node.z;
        float z2 = target.z;

        return Math.Abs(x1 - x2) + Math.Abs(y1 - y2) + Math.Abs(z1 - z2);
    }
}
