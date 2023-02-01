using Leopotam.Ecs;
using StarGravity.GamePlay.Common.Components;
using StarGravity.GamePlay.Interactables.MovingObjects.Components;
using UnityEngine;

namespace StarGravity.GamePlay.Interactables.MovingObjects.Systems
{
  public sealed class MoveObjectSystem : IEcsRunSystem
  {
    private readonly EcsFilter<MovingObjectComponent, TransformComponent> _movingObjectsFilter = null;
    public void Run()
    {
      foreach (var i in _movingObjectsFilter)
      {
        ref var movingObjectComponent = ref _movingObjectsFilter.Get1(i);
        ref var transformComponent = ref _movingObjectsFilter.Get2(i);
        
        if (movingObjectComponent.Speed != 0)
          transformComponent.Transform.Translate(movingObjectComponent.Direction * (movingObjectComponent.Speed * Time.deltaTime));
      }
    }
  }
}