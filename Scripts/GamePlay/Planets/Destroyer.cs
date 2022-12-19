using System;
using UnityEngine;

namespace StarGravity.GamePlay.Planets
{
  public class Destroyer : MonoBehaviour
  {
    private Renderer _renderer;
    private Vector2 _bottomLeftPoint;

    private void Awake()
    {
      _renderer = GetComponentInChildren<Renderer>();
      _bottomLeftPoint = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

    }

    private void Update()
    {
      if (transform.position.x + _renderer.bounds.size.x / 2 < _bottomLeftPoint.x)
        Destroy(gameObject);
    }
  }
}