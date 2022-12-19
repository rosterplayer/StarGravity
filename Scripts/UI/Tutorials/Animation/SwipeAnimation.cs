using DG.Tweening;
using UnityEngine;

namespace StarGravity.UI.Tutorials.Animation
{
  public class SwipeAnimation : MonoBehaviour
  {
    [SerializeField] private RectTransform _transform;
    [SerializeField] private float _animationDuration = 0.5f;
    [SerializeField] private float _intervalBetween = 0.3f;
    [SerializeField] private Vector3 _offset = Vector3.zero;
    
    private Sequence _clickAnimation;

    private void Start()
    {
      var originPosition = _transform.localPosition;
      _clickAnimation = DOTween.Sequence();
      _clickAnimation
        .Append(_transform.DOLocalMove(originPosition + _offset, _animationDuration))
        .Append(_transform.DOLocalMove(originPosition, 0.1f))
        .PrependInterval(_intervalBetween)
        .SetLoops(-1);
    }

    private void OnDisable()
    {
      _clickAnimation.Kill();
    }
  }
}