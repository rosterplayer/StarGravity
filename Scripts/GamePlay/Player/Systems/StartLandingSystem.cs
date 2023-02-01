using Leopotam.Ecs;
using StarGravity.GamePlay.Player.Components;
using UnityEngine;

namespace StarGravity.GamePlay.Player.Systems
{
  public class StartLandingSystem : IEcsRunSystem
  {
    private EcsFilter<StartLandingEvent, RigidbodyComponent, ShipState> _ships;

    public void Run()
    {
      foreach (int i in _ships)
      {
        ref var entity = ref _ships.GetEntity(i);
        ref var rigidbody = ref _ships.Get2(i);
        ref var shipState = ref _ships.Get3(i);

        shipState.State = PlayerState.OnLanding;
        rigidbody.Rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        
        entity.Del<Untouchable>();
        entity.Del<StartLandingEvent>();
      }
    }
  }
}