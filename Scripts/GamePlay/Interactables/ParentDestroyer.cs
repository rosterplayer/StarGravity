﻿using UnityEngine;

namespace StarGravity.GamePlay.Interactables
{
  public class ParentDestroyer : MonoBehaviour
  {
    private void Update()
    {
      if (transform.childCount <= 0)
        Destroy(gameObject);
    }
  }
}