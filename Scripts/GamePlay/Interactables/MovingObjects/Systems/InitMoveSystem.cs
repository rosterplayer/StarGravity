using Leopotam.Ecs;
using StarGravity.GamePlay.Common.Components;
using StarGravity.GamePlay.Interactables.MovingObjects.Components;

namespace StarGravity.GamePlay.Interactables.MovingObjects.Systems
{
  public class InitMoveSystem : IEcsRunSystem
  {
    private readonly EcsFilter<InitMoveRequest, TransformComponent> _initFilter;

    public void Run()
    {
      foreach (var i in _initFilter)
      {
        ref var entity = ref _initFilter.GetEntity(i);
        ref var transformComponent = ref _initFilter.Get2(i);
        
        var moveData = transformComponent.Transform.GetComponent<InitMoveData>();
        ref var movingObjectComponent = ref entity.Get<MovingObjectComponent>();
        movingObjectComponent.Direction = moveData.Direction;
        movingObjectComponent.Speed = moveData.Speed;
        
        entity.Del<InitMoveRequest>();
      }
    }
  }
}