using DG.Tweening;
using UnityEngine;

namespace StarGravity.GamePlay.Interactables
{
  [RequireComponent(typeof(Collider2D))]
  public class Dissolver : MonoBehaviour
  {
    public SpriteRenderer Renderer;
    public float Duration;
    private static readonly int Fade = Shader.PropertyToID("_Fade");
    private Tweener _tween;

    private void OnTriggerEnter2D(Collider2D col)
    {
      if (col.CompareTag("Player"))
        Dissolve();
    }

    private void Dissolve()
    {
      _tween = DOVirtual
        .Float(1, 0, Duration, fade =>
        {
          if (Renderer != null) Renderer.material.SetFloat(Fade, fade);
        });
    }

    private void OnDestroy()
    {
      _tween.Kill();
    }
  }
}