using System;
using StarGravity.Infrastructure.Factories;
using StarGravity.Infrastructure.Services.Sound;
using UnityEngine;
using VContainer;

namespace StarGravity.GamePlay.Player.Perks
{
  public class CaptureField : ShipPerkWithCooldown
  {
    private PoolOfBeams _poolOfBeams;
    private IGameObjectFactory _objectFactory;
    private SoundService _soundService;

    [Inject]
    public void Construct(IGameObjectFactory factory, SoundService soundService, Func<Transform, CaptureBeam> beamFactory)
    {
      _soundService = soundService;
      _objectFactory = factory;
      _poolOfBeams = new PoolOfBeams(beamFactory);
    }

    protected override void Activate()
    {
      if (IsCooldown())
        return;

      foreach (GameObject bonus in Magnet.GetBonusesForCapture(transform.position, _objectFactory.Bonuses))
      {
        var captureBeam = _poolOfBeams.ShowBeam(bonus.transform.position, transform);
        Magnet.Capture(gameObject, bonus, () => _poolOfBeams.ReleaseBeam(captureBeam));
      }
      
      SetCooldown();
      _soundService.PlaySFX(SfxType.CaptureField);
    }
  }
}