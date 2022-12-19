using DG.Tweening;
using UnityEngine;

namespace StarGravity.GamePlay.Interactables.Animations
{
  public class CollectableAnimation : MonoBehaviour
  {
    public Ease Ease;

    private void Start()
    {
      transform.DOScale(0.4f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease);
    }

    private void OnDestroy()
    {
      transform.DOKill();
    }
  }
}