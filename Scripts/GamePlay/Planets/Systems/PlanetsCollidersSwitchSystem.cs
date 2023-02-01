using Leopotam.Ecs;
using StarGravity.GamePlay.Common.Components;
using StarGravity.GamePlay.Planets.Components;

namespace StarGravity.GamePlay.Planets.Systems
{
  public sealed class PlanetsCollidersSwitchSystem : IEcsRunSystem
  {
    private EcsFilter<DestinationTag, TransformComponent> _filter;
    private EcsFilter<ShipLeftPlanetEvent> _events;

    public void Run()
    {
      if (_events.IsEmpty())
        return;
      
      foreach (int i in _filter)
      {
        ref var transform = ref _filter.Get2(i);
        
        transform.Transform.gameObject.GetComponentInChildren<Destination>().SwitchOnColliderWithDelay();
      }

      foreach (int i in _events)
      {
        _events.GetEntity(i).Del<ShipLeftPlanetEvent>();
      }
    }
  }
}