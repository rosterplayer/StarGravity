using Leopotam.Ecs;
using StarGravity.GamePlay.Common.Components;
using StarGravity.GamePlay.Player.Components;

namespace StarGravity.GamePlay.Player.Systems
{
  public sealed class FlyAfterRespawnSystem: IEcsRunSystem
  {
    private const float HorizontalSpeed = 5;
    
    private EcsFilter<ShipState, RigidbodyComponent, TransformComponent, ReachedPlanet> _ships;

    public void Run()
    {
      foreach (int i in _ships)
      {
        ref var shipState = ref _ships.Get1(i);
        
        if (shipState.State != PlayerState.OnLevelStart)
          continue;

        ref var rigidbody = ref _ships.Get2(i);
        ref var transform = ref _ships.Get3(i);
        ref var reachedPlanet = ref _ships.Get4(i);
        
        rigidbody.Rigidbody.velocity = (reachedPlanet.Planet.transform.position - transform.Transform.position).normalized * HorizontalSpeed;
        shipState.State = PlayerState.OnFLy;
      }
    }
  }
}