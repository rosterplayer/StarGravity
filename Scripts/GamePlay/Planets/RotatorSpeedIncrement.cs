using StarGravity.Infrastructure.Services.Progress;
using UnityEngine;
using VContainer;

namespace StarGravity.GamePlay.Planets
{
  [RequireComponent(typeof(Rotator))]
  public class RotatorSpeedIncrement : MonoBehaviour, IResetable
  {
    [SerializeField] private Rotator _rotator;
    private IGameLevelProgressService _levelProgressService;

    [Inject]
    public void Construct(IGameLevelProgressService levelProgress)
    {
      _levelProgressService = levelProgress;
      AdjustMaxRotatorSpeed();
    }

    public void Reset()
    {
      AdjustMaxRotatorSpeed();
    }

    private void AdjustMaxRotatorSpeed()
    {
      float range = _rotator.MaxRotateSpeed - _rotator.MinRotateSpeed;
      switch (_levelProgressService.LevelProgress.Systems)
      {
        case > 10:
          return;
        case > 6:
          _rotator.MaxRotateSpeed = _rotator.MinRotateSpeed + 0.75f * range;
          return;
        case > 3:
          _rotator.MaxRotateSpeed = _rotator.MinRotateSpeed + 0.5f * range;
          return;
        default:
          _rotator.MaxRotateSpeed = _rotator.MinRotateSpeed + 0.25f * range;
          break;
      }
    }
  }
}