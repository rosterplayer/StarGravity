using System;
using UnityEngine;

namespace StarGravity.GamePlay.Utilities
{
  public static class PathInterpolating
  {
    public static Vector3 Interpolate(this Vector3[] path, float t)
    {
      int numSections = path.Length - 3;
      int currPt = Mathf.Min(Mathf.FloorToInt(t * numSections), numSections - 1);
      float u = t * numSections - currPt;
      Vector3 a = path[currPt];
      Vector3 b = path[currPt + 1];
      Vector3 c = path[currPt + 2];
      Vector3 d = path[currPt + 3];
      return 0.5f * ((-a + 3f * b - 3f * c + d) * (u * u * u) + (2f * a - 5f * b + 4f * c - d) * (u * u) + (-a + c) * u + 2f * b);
    }

    public static Vector3[] CreatePoints(this Vector3[] path)
    {
      int dist = 2;
      Vector3[] pathPositions = path;
      Vector3[] newPathPos = new Vector3[pathPositions.Length + dist];
        
      Array.Copy(pathPositions, 0, newPathPos, 1, pathPositions.Length);
      newPathPos[0] = newPathPos[1] + (newPathPos[1] - newPathPos[2]);
      newPathPos[^1] = newPathPos[^2] + (newPathPos[^2] - newPathPos[^3]);
      if (newPathPos[1] == newPathPos[^2])
      {
        Vector3[] loopSpline = new Vector3[newPathPos.Length];
        Array.Copy(newPathPos, loopSpline, newPathPos.Length);
        loopSpline[0] = loopSpline[^3];
        loopSpline[^1] = loopSpline[2];
        newPathPos = new Vector3[loopSpline.Length];
        Array.Copy(loopSpline, newPathPos, loopSpline.Length);
      }
      return (newPathPos);
    }
  }
}