using System;
using System.Collections.Generic;
using UnityEngine;

namespace StarGravity.Data
{
  public class ForPlanetsPositions : MonoBehaviour
  {
    public PlanetsPosition[] ForPositions;
  }

  [Serializable]
  public class PlanetsPosition
  {
    public PositionOnScreen LeftPlanet;
    public PositionOnScreen Star;
    public PositionOnScreen RightPlanet;

    public PlanetsPosition(PositionOnScreen leftPlanet, PositionOnScreen star, PositionOnScreen rightPlanet)
    {
      LeftPlanet = leftPlanet;
      Star = star;
      RightPlanet = rightPlanet;
    }
  }

  public class PlanetPositionComparer : IEqualityComparer<PlanetsPosition>
  {
    public bool Equals(PlanetsPosition x, PlanetsPosition y)
    {
      if (ReferenceEquals(x, y)) return true;
      if (ReferenceEquals(x, null)) return false;
      if (ReferenceEquals(y, null)) return false;
      if (x.GetType() != y.GetType()) return false;
      return x.LeftPlanet == y.LeftPlanet && x.Star == y.Star && x.RightPlanet == y.RightPlanet;
    }

    public int GetHashCode(PlanetsPosition obj)
    {
      return HashCode.Combine((int)obj.LeftPlanet, (int)obj.Star, (int)obj.RightPlanet);
    }
  }
}