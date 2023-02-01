using DG.Tweening;
using Leopotam.Ecs;
using StarGravity.GamePlay.Common.Components;
using StarGravity.GamePlay.Player.Components;
using UnityEngine;

namespace StarGravity.GamePlay.Player.Systems
{
  public class RespawnSystem : IEcsRunSystem
  {
    private EcsFilter<NeedRespawn, TransformComponent, RigidbodyComponent>.Exclude<Untouchable> _respawnFilter;
    public void Run()
    {
      foreach (int i in _respawnFilter)
      {
        ref var transform = ref _respawnFilter.Get2(i);
        ref var rigidbody = ref _respawnFilter.Get3(i);
        ref var entity = ref _respawnFilter.GetEntity(i);
        
        transform.Transform.DOKill();

        if (transform.Transform.TryGetComponent(out Health health))
        {
          health.DecreaseHealth();
        }
        
        entity.Get<Untouchable>();
        rigidbody.Rigidbody.velocity = Vector2.zero;
        rigidbody.Rigidbody.angularVelocity = 0;

        ref var startPosition = ref entity.Get<StartPosition>();
        transform.Transform.position = startPosition.Position;

        ref var shipState = ref entity.Get<ShipState>();
        shipState.State = PlayerState.OnLevelStart;
        
        transform.Transform.GetComponent<StarShip>().OnRespawn();
      }
    }
  }
}