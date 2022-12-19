using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StarGravity.UI.Essentials.Buttons
{
  [RequireComponent(typeof(CanvasGroup))]
  [RequireComponent(typeof(FadeConfig))]
  public class FadeButton : ButtonWithSound
  {
    private FadeConfig _fadeConfig;
    private CanvasGroup _canvasGroup;
    private TweenerCore<float, float, FloatOptions> _fadeTween;

    protected override void Awake()
    {
      base.Awake();
      _fadeConfig = GetComponent<FadeConfig>();
      _canvasGroup = GetComponent<CanvasGroup>();
    }
    
    public override void OnPointerEnter(PointerEventData eventData)
    {
      base.OnPointerEnter(eventData);
      _fadeTween.Complete();
      _fadeTween = _canvasGroup.DOFade(_fadeConfig.OnHoverAlpha, _fadeConfig.FadeTime);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
      base.OnPointerExit(eventData);
      _fadeTween.Complete();
      _fadeTween = _canvasGroup.DOFade(1.0f, _fadeConfig.FadeTime);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
      base.OnPointerDown(eventData);
      _canvasGroup.alpha = _fadeConfig.OnClickAlpha;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
      base.OnPointerUp(eventData);
      _canvasGroup.alpha = 1.0f;
    }

    protected override void OnDestroy()
    {
      _fadeTween.Kill();
    }
  }
}