using System;
using UnityEngine;

namespace StarGravity.GamePlay.Planets.Components
{
  [Serializable]
  public struct MovablePlanet
  {
    public float Speed;
    public MovePoints MovePoints;
    public Vector2 CurrenDestination;
  }
}