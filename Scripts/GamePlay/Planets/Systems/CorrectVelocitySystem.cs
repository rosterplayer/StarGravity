using Leopotam.Ecs;
using StarGravity.GamePlay.Interactables.MovingObjects.Components;
using StarGravity.GamePlay.Planets.Components;
using UnityEngine;

namespace StarGravity.GamePlay.Planets.Systems
{
  public class CorrectVelocitySystem : IEcsRunSystem
  {
    private EcsFilter<CorrectVelocityRequest> _requestFilter;
    private EcsFilter<MovingObjectComponent> _movesFilter;

    public void Run()
    {
      if (_requestFilter.IsEmpty())
        return;

      foreach (int i in _requestFilter)
      {
        ref var correction = ref _requestFilter.Get1(i);
        ref var correctionEntity = ref _requestFilter.GetEntity(i);

        foreach (int j in _movesFilter)
        {
          CorrectVelocity(j, correction.CorrectionVelocity);
        }
        
        correctionEntity.Del<CorrectVelocityRequest>();
      }
    }

    private void CorrectVelocity(int idx, Vector2 correction)
    {
      ref var movingObject = ref _movesFilter.Get1(idx);

      Vector2 newDirection = movingObject.Direction * movingObject.Speed + correction;
      movingObject.Direction = newDirection.normalized;
      movingObject.Speed = newDirection.magnitude;
    }
  }
}