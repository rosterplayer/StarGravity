using Leopotam.Ecs;
using StarGravity.GamePlay.Common.Components;
using UnityEngine;

namespace StarGravity.GamePlay.Common.Systems
{
  public class RotateSystem : IEcsRunSystem
  {
    private readonly EcsFilter<RotatorComponent> _rotateFilter;
    
    public void Run()
    {
      foreach (int i in _rotateFilter)
      {
        ref var rotator = ref _rotateFilter.Get1(i);
        
        if (rotator.CurrentRotationSpeed == 0)
          continue;
        
        rotator.RotatingTransform.Rotate(0, 0, rotator.CurrentRotationSpeed * Time.deltaTime);
      }
    }
  }
}