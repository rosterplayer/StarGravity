using System;
using System.Collections.Generic;
using StarGravity.Infrastructure.Factories;
using StarGravity.Infrastructure.Services.Sound;
using UnityEngine;
using VContainer;

namespace StarGravity.GamePlay.Player.Perks
{
  public class CaptureField : ShipPerkWithCooldown
  {
    private PoolOfBeams _poolOfBeams;
    private GameObjectFactory _objectFactory;
    private SoundService _soundService;

    [Inject]
    public void Construct(GameObjectFactory factory, SoundService soundService, Func<Transform, CaptureBeam> beamFactory)
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

  public class PoolOfBeams
  {
    private readonly List<CaptureBeam> _pool = new(6);
    private readonly Func<Transform, CaptureBeam> _beamFactory;

    public PoolOfBeams(Func<Transform, CaptureBeam> beamFactory)
    {
      _beamFactory = beamFactory;
    }
    
    public CaptureBeam ShowBeam(Vector3 to, Transform parent)
    {
      CaptureBeam beam = GetReleasedBeam(parent);
      beam.Show(to);
      return beam;
    }

    private CaptureBeam GetReleasedBeam(Transform parent)
    {
      CaptureBeam releasedBeam = _pool.Find(x => x.IsActive == false);
      if (releasedBeam == null)
      {
        releasedBeam = _beamFactory(parent);
        _pool.Add(releasedBeam);
      }

      return releasedBeam;
    }

    public void ReleaseBeam(CaptureBeam beam)
    {
      beam.Hide();
    }
  }
}