using DG.Tweening;
using UnityEngine;

namespace StarGravity.UI.Tutorials.Animation
{
  public class ClickAnimation : MonoBehaviour
  {
    [SerializeField] private RectTransform _transform;
    [SerializeField] private float _animationDuration = 0.5f;
    [SerializeField] private float _intervalBetweenClicks = 0.3f;
    [SerializeField] private float _scaleTo = 0.8f;
    private Sequence _clickAnimation;

    private void Start()
    {
      _clickAnimation = DOTween.Sequence();
      _clickAnimation
        .Append(_transform.DOScale(_scaleTo, _animationDuration))
        .Append(_transform.DOScale(1f, 0.1f))
        .PrependInterval(_intervalBetweenClicks)
        .SetLoops(-1);
    }

    private void OnDisable()
    {
      _clickAnimation.Kill();
    }
  }
}