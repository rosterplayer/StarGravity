using DG.Tweening;
using UnityEngine;

namespace StarGravity.GamePlay.Player.Animations
{
  public class FlashingAnimation : MonoBehaviour
  {
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Tweener _flashing;

    public void StartAnimation() => 
      _flashing = _spriteRenderer.DOFade(0, 0.3f).SetLoops(-1, LoopType.Yoyo);

    public void StopAnimation()
    {
      _flashing.Kill();
      _spriteRenderer.DOFade(1, 0.1f);
    }
    
    private void OnDestroy()
    {
      _flashing?.Kill();
    }
  }
}