using DG.Tweening;
using StarGravity.Infrastructure.Services.Sound;
using UnityEngine;
using VContainer;

namespace StarGravity.GamePlay.Player.Perks
{
  public class InvisibilityField : ShipPerkWithCooldown
  {
    public SpriteRenderer Renderer;
    public CircleCollider2D Collider2D;
    
    [Tooltip("How long lasts invisibility")]
    public float Duration;
    
    private static readonly int Fade = Shader.PropertyToID("_Fade");
    private SoundService _soundService;

    [Inject]
    public void Construct(SoundService soundService)
    {
      _soundService = soundService;
    }
    
    protected override void Activate()
    {
      if (IsCooldown())
        return;
      
      SetCooldown();
      SwitchOnInvisibility();
      _soundService.PlaySFX(SfxType.Invisibility);
    }

    private void SwitchOnInvisibility()
    {
      const float fade = 1;
      Collider2D.enabled = false;
      DOVirtual
        .Float(90, 270, Duration, angle =>
        {
          if (Renderer != null) Renderer.material.SetFloat(Fade, fade + Mathf.Cos(Mathf.Deg2Rad * angle));
        })
        .OnComplete(() =>
        {
          if (Collider2D != null) Collider2D.enabled = true;
        });
    }
  }
}