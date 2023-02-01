using System;
using System.Collections.Generic;
using UnityEngine;

namespace StarGravity.GamePlay.Planets
{
  public class ResetableChilds : MonoBehaviour
  {
    private List<IResetable> _resetables;

    private void Awake()
    {
      _resetables = new List<IResetable>();
      _resetables.AddRange(gameObject.GetComponentsInChildren<IResetable>());
    }

    public void ResetAll()
    {
      foreach (IResetable resetable in _resetables)
      {
        resetable.Reset();
      }
    }
  }
}