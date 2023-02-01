using Leopotam.Ecs;
using StarGravity.GamePlay.Background.Components;
using StarGravity.GamePlay.Common.Components;
using StarGravity.GamePlay.Planets.Components;
using UnityEngine;

namespace StarGravity.GamePlay.Background.Systems
{
  public class HorizontalMovingSystem : IEcsRunSystem
  {
    private EcsFilter<IsMoving, HorizontalMovingComponent, TransformComponent> _movers;

    public void Run()
    {
      foreach (int i in _movers)
      {
        ref var horizontalMoving = ref _movers.Get2(i);
        ref var transform = ref _movers.Get3(i);
        
        transform.Transform.Translate(Vector3.left * horizontalMoving.Speed * Time.deltaTime);
      }      
    }
  }
}