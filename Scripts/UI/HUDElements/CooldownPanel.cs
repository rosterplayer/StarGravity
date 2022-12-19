using DG.Tweening;
using StarGravity.GamePlay.Player.Perks;
using UnityEngine;
using UnityEngine.UI;

namespace StarGravity.UI.HUDElements
{
  public class CooldownPanel : MonoBehaviour
  {
    public ToggleGroup Slider;
    public Image Icon;

    private float _cooldown;
    private Tweener _tweener;
    private CanvasGroup _canvasGroup;

    public void Initialize(ShipPerkWithCooldown perk)
    {
      _cooldown = perk.Cooldown;
      perk.OnStartCooldown += StartCooldown;
    }

    private void Start()
    {
      _canvasGroup = Slider.GetComponent<CanvasGroup>();
    }

    private void StartCooldown()
    {
      _tweener = DOVirtual
        .Float(0, 1, _cooldown, value =>
        {
          Slider.Switch(Mathf.FloorToInt(Slider.Toggles.Length * value));
        })
        .OnStart(() =>
        {
          float flashDuration = 0.3f;
          int loopsCount = (int)(_cooldown / flashDuration);
          _canvasGroup.DOFade(0.5f, flashDuration).SetLoops(loopsCount % 2 == 0 ? loopsCount : loopsCount - 1, LoopType.Yoyo);
        })
        .OnComplete(() =>
        {
          _canvasGroup.DOComplete();
        });
    }

    private void OnDestroy()
    {
      _tweener?.Complete();
    }
  }
}