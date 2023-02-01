using Leopotam.Ecs;
using StarGravity.GamePlay.Background.Components;
using StarGravity.GamePlay.Planets.Components;

namespace StarGravity.GamePlay.Background.Systems
{
  public class StartHorizontalMovingSystem : IEcsRunSystem
  {
    private EcsFilter<PlanetsMovingStarted> _events;
    private EcsFilter<HorizontalMovingComponent> _movables;


    public void Run()
    {
      if (_events.IsEmpty())
        return;

      foreach (int i in _movables)
      {
        ref var entity = ref _movables.GetEntity(i);
        entity.Get<IsMoving>();
      }
    }
  }
}