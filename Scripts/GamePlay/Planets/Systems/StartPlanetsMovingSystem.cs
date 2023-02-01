using Leopotam.Ecs;
using StarGravity.GamePlay.Common.Components;
using StarGravity.GamePlay.Planets.Components;
using StarGravity.Infrastructure.Services.Input;
using UnityEngine;

namespace StarGravity.GamePlay.Planets.Systems
{
  public sealed class StartPlanetsMovingSystem : IEcsRunSystem
  {
    private IInputService _inputService;
    
    private EcsFilter<PlanetsMovingStarted> _events;
    private EcsFilter<MovablePlanet, TransformComponent> _movables;
    
    public void Run()
    {
      if (_events.IsEmpty())
        return;

      foreach (int i in _movables)
      {
        ref var movable = ref _movables.Get1(i);
        ref var transform = ref _movables.Get2(i);
        ref var entity = ref _movables.GetEntity(i);

        SetNextDestination(ref movable, ref transform);
        entity.Get<IsMoving>();
      }

      foreach (int i in _events)
      {
        _events.GetEntity(i).Del<PlanetsMovingStarted>();
      }
      
      _inputService.ReleaseControl();
    }

    private void SetNextDestination(ref MovablePlanet movable, ref TransformComponent transform)
    {
      float? destinationPoint = movable.MovePoints.GetNextPointX();

      if (destinationPoint == null)
        return;
      
      movable.CurrenDestination = new Vector2((float)destinationPoint, transform.Transform.position.y);
    }
  }
}