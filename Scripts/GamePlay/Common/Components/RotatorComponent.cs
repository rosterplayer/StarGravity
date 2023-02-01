using System;
using UnityEngine;

namespace StarGravity.GamePlay.Common.Components
{
  [Serializable]
  public struct RotatorComponent
  {
    public Transform RotatingTransform;
    public float MinRotateSpeed;
    public float MaxRotateSpeed;
    public float CurrentRotationSpeed;

    public bool RandomizeRotatingSpeed;
  }
}