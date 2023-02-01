using Leopotam.Ecs;
using StarGravity.GamePlay.Common.Components;
using StarGravity.GamePlay.Planets.Components;
using StarGravity.GamePlay.Player.Components;
using StarGravity.GamePlay.Player.Input.Components;
using UnityEngine;

namespace StarGravity.GamePlay.Player.Systems
{
  public sealed class JumpFromOrbitSystem : IEcsRunSystem
  {
    private const int JumpPower = 12;

    private EcsWorld _world;
    private EcsFilter<JumpPressed, TransformComponent, RigidbodyComponent, ShipState> _shipFilter;

    public void Run()
    {
      foreach (int i in _shipFilter)
      {
        ref var transform = ref _shipFilter.Get2(i);
        ref var rigidbody = ref _shipFilter.Get3(i);
        ref var shipState = ref _shipFilter.Get4(i);

        rigidbody.Rigidbody.constraints = RigidbodyConstraints2D.None;
        transform.Transform.SetParent(null);
        shipState.State = PlayerState.OnFLy;
        rigidbody.Rigidbody.AddForce(transform.Transform.up * JumpPower, ForceMode2D.Impulse);

        _world.NewEntity().Get<ShipLeftPlanetEvent>();
      }
    }
  }
}