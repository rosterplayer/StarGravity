using Leopotam.Ecs;
using StarGravity.GamePlay.Common.Components;
using StarGravity.GamePlay.Player.Components;
using UnityEngine;

namespace StarGravity.GamePlay.Player.Systems
{
  public sealed class ShipRotatingSystem : IEcsRunSystem
  {
    private EcsFilter<RigidbodyComponent, TransformComponent, ShipState> _shipFilter;

    public void Run()
    {
      foreach (int i in _shipFilter)
      {
        ref var rigidbody = ref _shipFilter.Get1(i);
        ref var transform = ref _shipFilter.Get2(i);
        ref var shipState = ref _shipFilter.Get3(i);
        
        if (rigidbody.Rigidbody.velocity == Vector2.zero || shipState.State != PlayerState.OnFLy)
          continue;
        
        transform.Transform.rotation = Quaternion.LookRotation(Vector3.forward, rigidbody.Rigidbody.velocity.normalized);
      }
    }
  }
}