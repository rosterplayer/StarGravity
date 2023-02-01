using Leopotam.Ecs;
using StarGravity.GamePlay.Common.Components;
using StarGravity.GamePlay.Interactables.MovingObjects.Components;
using UnityEngine;

namespace StarGravity.GamePlay.Interactables.MovingObjects.Systems
{
  public class FollowSystem : IEcsRunSystem
  {
    private const int CaptureSpeed = 20;
    
    private EcsFilter<FollowComponent, MovingObjectComponent, TransformComponent> _followers;
    
    public void Run()
    {
      foreach (int i in _followers)
      {
        ref var followComponent = ref _followers.Get1(i);
        ref var moveComponent = ref _followers.Get2(i);
        ref var transformComponent = ref _followers.Get3(i);

        Vector2 newDirection = (followComponent.FollowTo.position - transformComponent.Transform.position).normalized;
        moveComponent.Direction = newDirection;
        moveComponent.Speed = CaptureSpeed;
      }      
    }
  }
}