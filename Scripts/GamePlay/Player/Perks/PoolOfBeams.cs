using System;
using System.Collections.Generic;
using UnityEngine;

namespace StarGravity.GamePlay.Player.Perks
{
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