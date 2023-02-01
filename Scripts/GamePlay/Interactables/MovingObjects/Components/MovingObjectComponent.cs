using System;
using UnityEngine;

namespace StarGravity.GamePlay.Interactables.MovingObjects.Components
{
  [Serializable]
  public struct MovingObjectComponent
  {
    public float Speed;
    public Vector2 Direction;
  }
}