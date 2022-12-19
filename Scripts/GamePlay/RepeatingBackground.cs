using System;
using UnityEngine;

namespace StarGravity.GamePlay
{
  /// <summary>
  /// This script attaches to ‘Background’ object, and would move it up if the object went down below the viewport border. 
  /// This script is used for creating the effect of infinite movement. 
  /// </summary>
  public class RepeatingBackground : MonoBehaviour
  {
    [Tooltip("vertical size of the sprite in the world space. Attach box collider2D to get the exact size")]
    public float HorizontalSize;

    private bool _cameraMoving;
    private Camera _camera;

    private void Awake()
    {
      _camera = Camera.main; 
    }

    private void Update()
    {
      Vector2 bottomLeftPoint = _camera.ViewportToWorldPoint(new Vector2(0, 0));
      if (transform.position.x + HorizontalSize < bottomLeftPoint.x) //if sprite goes down below the viewport move the object up above the viewport
      {
        RepositionBackground();
      }
    }

    private void RepositionBackground() 
    {
      Vector2 groundOffSet = new Vector2(HorizontalSize * 4f, 0);
      transform.position = (Vector2)transform.position + groundOffSet;
    }
  }
}