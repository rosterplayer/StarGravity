using Leopotam.Ecs;
using StarGravity.GamePlay.Background.Components;
using StarGravity.GamePlay.Common.Components;
using UnityEngine;

namespace StarGravity.GamePlay.Background.Systems
{
  public class RepositionSystem : IEcsRunSystem
  {
    private EcsFilter<RepositionFlag, TransformComponent,CheckOutOfScreenComponent> _filter;
    public void Run()
    {
      foreach (int i in _filter)
      {
        ref var transform = ref _filter.Get2(i);
        ref var outOfScreen = ref _filter.Get3(i);
        ref var entity = ref _filter.GetEntity(i);
        
        Vector2 groundOffSet = new Vector2(outOfScreen.LeftOffset * 4f, 0);
        transform.Transform.position = (Vector2)transform.Transform.position + groundOffSet;
        entity.Del<RepositionFlag>();
      }
    }
  }
}