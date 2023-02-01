using Leopotam.Ecs;
using StarGravity.GamePlay.Common.Components;
using StarGravity.GamePlay.Planets.Components;
using UnityEngine;

namespace StarGravity.GamePlay.Planets.Systems
{
  public sealed class MovePlanetSystem : IEcsRunSystem
  {
    private EcsWorld _world;
    private EcsFilter<IsMoving, MovablePlanet, TransformComponent>.Exclude<NotActive> _movables;

    public void Run()
    {
      foreach (int i in _movables)
      {
        ref var movable = ref _movables.Get2(i);
        ref var transform = ref _movables.Get3(i);
        ref var entity = ref _movables.GetEntity(i);

        transform.Transform.Translate(Vector3.left * movable.Speed * Time.deltaTime);
        // var position = transform.Transform.position;
        // position = Vector3.Lerp(position, position + Vector3.left * movable.Speed, Time.deltaTime);
        // transform.Transform.position = position;

        if (transform.Transform.position.x <= movable.CurrenDestination.x)
        {
          entity.Del<IsMoving>();
          if (entity.Has<PlanetWithPlayer>())
            _world.NewEntity().Get<PlanetsMovingEnded>();
        }
      }
    }
  }
}