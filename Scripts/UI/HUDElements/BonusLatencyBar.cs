using DG.Tweening;
using UltimateClean;
using UnityEngine;


namespace StarGravity.UI.HUDElements
{
  public class BonusLatencyBar : MonoBehaviour
  {
    public SlicedFilledImage Image;
    public CanvasGroup Bar;
    
    private Tweener _tween;

    public void OnStart(float timeRemain)
    {
      ShowBar();
      _tween.Kill();
      _tween = DOVirtual.Float(1, 0, timeRemain, fill =>
      {
        Image.fillAmount = fill;
      }).OnComplete(HideBar);
    }

    private void ShowBar()
    {
      gameObject.SetActive(true);
      Bar.DOFade(1, 0.2f);
    }

    private void HideBar() => 
      Bar.DOFade(0, 0.2f).OnComplete(() => gameObject.SetActive(false));

    private void OnDestroy()
    {
      _tween?.Kill();
      Bar.DOKill();
    }
  }
}