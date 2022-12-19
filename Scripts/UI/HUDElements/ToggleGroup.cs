using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace StarGravity.UI.HUDElements
{
  public class ToggleGroup : MonoBehaviour
  {
    public Toggle[] Toggles;
    [Tooltip("Turn on animation on first toggle when it only one active")]
    [SerializeField] private bool _animateLastToggle;
    private int _last;

    public void Switch(int count)
    {
      if (count == _last)
        return;

      _last = count;
      for (int i = 0; i < Toggles.Length; i++)
      {
        Toggles[i].isOn = i < count;
      }

      if (_animateLastToggle && count == 1)
      {
        Toggles[0].graphic.DOFade(0.5f, 0.3f).SetLoops(-1, LoopType.Yoyo);
      }
    }

    private void OnDestroy()
    {
      if (_animateLastToggle)
      {
        Toggles[0].graphic.DOKill();
      }
    }
  }
}