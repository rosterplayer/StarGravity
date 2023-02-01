using Leopotam.Ecs;
using StarGravity.GamePlay.Common.Components;
using UnityEngine;

namespace StarGravity.GamePlay.Common.Systems
{
  sealed class DestroyObjectsSystem : IEcsRunSystem
  {
    private readonly EcsFilter<ForDestroy, TransformComponent> _destroyFilter = null;

    public void Run()
    {
      foreach (var i in _destroyFilter)
      {
        ref var entity = ref _destroyFilter.GetEntity(i);
        ref var transform = ref _destroyFilter.Get2(i);
        
        Object.Destroy(transform.Transform.gameObject);
        entity.Destroy();
      }
    }
  }
}