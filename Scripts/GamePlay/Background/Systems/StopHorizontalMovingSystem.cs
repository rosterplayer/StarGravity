using Leopotam.Ecs;
using StarGravity.GamePlay.Background.Components;
using StarGravity.GamePlay.Planets.Components;
using UnityEngine;

namespace StarGravity.GamePlay.Background.Systems
{
  public class StopHorizontalMovingSystem : IEcsRunSystem
  {
    private EcsFilter<PlanetsMovingEnded> _events;
    private EcsFilter<IsMoving,HorizontalMovingComponent> _movables;


    public void Run()
    {
      if (_events.IsEmpty())
        return;
      
      foreach (int i in _movables)
      {
        ref var entity = ref _movables.GetEntity(i);
        entity.Del<IsMoving>();
      }
    }
  }
}