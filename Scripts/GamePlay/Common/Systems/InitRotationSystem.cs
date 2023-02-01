using Leopotam.Ecs;
using StarGravity.GamePlay.Common.Components;
using UnityEngine;

namespace StarGravity.GamePlay.Common.Systems
{
  public sealed class InitRotationSystem : IEcsRunSystem
  {
    private EcsFilter<InitRotation, RotatorComponent> _filter;
    public void Run()
    {
      foreach (int i in _filter)
      {
        ref var rotatorComponent = ref _filter.Get2(i);
        ref var entity = ref _filter.GetEntity(i);
        rotatorComponent.CurrentRotationSpeed = GetRandomRotationSpeed(rotatorComponent);
        entity.Del<InitRotation>();
      }
    }

    private float GetRandomRotationSpeed(RotatorComponent rotatorComponent)
    {
      float clockwise = Random.Range(0, 100) < 50 ? -1f : 1f;
      float rotationSpeed = rotatorComponent.RandomizeRotatingSpeed 
        ? Random.Range(rotatorComponent.MinRotateSpeed, rotatorComponent.MaxRotateSpeed) 
        : rotatorComponent.MaxRotateSpeed;

      return rotationSpeed * clockwise;
    }
  }
}