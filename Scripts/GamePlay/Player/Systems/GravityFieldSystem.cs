using Leopotam.Ecs;
using StarGravity.GamePlay.Common.Components;
using StarGravity.GamePlay.Player.Components;
using UnityEngine;

namespace StarGravity.GamePlay.Player.Systems
{
  public sealed class GravityFieldSystem : IEcsRunSystem
  {
    private const float GravityMinForce = 8;
    
    private EcsFilter<InGravityField, TransformComponent, RigidbodyComponent>.Exclude<Untouchable> _filter;

    public void Run()
    {
      foreach (int i in _filter)
      {
        ref var gravityField = ref _filter.Get1(i);
        ref var transform = ref _filter.Get2(i);
        ref var rigidbody = ref _filter.Get3(i);
        
        Vector2 direction = gravityField.GravityFieldCenter - transform.Transform.position;
        float force = GravityMinForce * gravityField.GravityFieldRadius / direction.magnitude;
        direction.Normalize();
        rigidbody.Rigidbody.AddForce(direction * force, ForceMode2D.Force);
      }
    }
  }
}