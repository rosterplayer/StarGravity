using StarGravity.Infrastructure.Services.Sound;
using UnityEngine;
using VContainer;

namespace StarGravity.GamePlay.Player.Perks
{
  [RequireComponent(typeof(StarShip))]
  public class Shield : ShipPerkWithCooldown
  {
    private const float ShieldActiveTime = 1f;
    
    public ParticleSystem ShieldParticle;

    private bool _isActive;
    private float _activeTime;
    private SoundService _soundService;

    public bool IsActive => _isActive;

    [Inject]
    public void Construct(SoundService soundService)
    {
      _soundService = soundService;
    }

    protected override void Activate()
    {
      if (IsCooldown())
        return;
      
      ShieldActive(true);
      _activeTime = ShieldActiveTime;
      ShieldParticle.Play();
      SetCooldown();
      _soundService.PlaySFX(SfxType.Shield);
    }

    protected override void OnUpdate()
    {
      if (_isActive && _activeTime <= 0)
      {
        ShieldActive(false);
      }

      _activeTime -= Time.deltaTime;
    }

    private void ShieldActive(bool on)
    {
      _isActive = on;
    }
  }
}