using Leopotam.Ecs;
using StarGravity.GamePlay.Background.Components;
using StarGravity.GamePlay.Planets.Components;
using StarGravity.GamePlay.Player.Components;
using StarGravity.GamePlay.Stars.Components;
using UnityEngine;

namespace StarGravity.GamePlay.Common.Components
{
  sealed class CheckOutOfScreenSystem : IEcsInitSystem,IEcsRunSystem
  {
    private Vector2 _cameraBottomLeftPoint;
    private Vector2 _cameraTopRightPoint;

    private readonly EcsFilter<CheckOutOfScreenComponent, TransformComponent>.Exclude<NotActive> _movableFilter = null;
    
    public void Init()
    {
      Camera camera = Camera.main;

      if (camera == null)
        return;
      
      _cameraBottomLeftPoint = camera.ViewportToWorldPoint(new Vector2(0, 0));
      _cameraTopRightPoint = camera.ViewportToWorldPoint(new Vector2(1, 1));
    }

    public void Run()
    {
      foreach (var i in _movableFilter)
      {
        ref var checkOutOfScreenComponent = ref _movableFilter.Get1(i);
        ref var transformComponent = ref _movableFilter.Get2(i);
        ref EcsEntity entity = ref _movableFilter.GetEntity(i);


        if (IsOutOfScreen(transformComponent.Transform, checkOutOfScreenComponent))
          SetFlagComponent(entity, checkOutOfScreenComponent.IfOutAction);
        else
          UnsetFlagComponent(entity, checkOutOfScreenComponent.IfOutAction);
      }
    }

    private bool IsOutOfScreen(Transform transform, CheckOutOfScreenComponent offsets)
    {
      var position = transform.position;
      return position.x > _cameraTopRightPoint.x + offsets.RightOffset
             || position.y > _cameraTopRightPoint.y + offsets.TopOffset
             || position.x < _cameraBottomLeftPoint.x - offsets.LeftOffset
             || position.y < _cameraBottomLeftPoint.y - offsets.BottomOffset;
    }

    private static void SetFlagComponent(EcsEntity entity, OutOfScreenAction action)
    {
      switch (action)
      {
        case OutOfScreenAction.Nothing:
          break;
        case OutOfScreenAction.Destroy:
          entity.Get<ForDestroy>();
          break;
        case OutOfScreenAction.Respawn:
          entity.Get<CrashedEvent>();
          break;
        case OutOfScreenAction.Hide:
          ref var transformComponent = ref entity.Get<TransformComponent>();
          transformComponent.Transform.gameObject.SetActive(false);
          entity.Get<NotActive>();
          break;
        case OutOfScreenAction.Reposition:
          entity.Get<RepositionFlag>();
          break;
        case OutOfScreenAction.SwitchOff:
          entity.Get<SwitchOff>();
          break;
      }
    }

    private void UnsetFlagComponent(EcsEntity entity, OutOfScreenAction action)
    {
      if (action == OutOfScreenAction.SwitchOff)
        entity.Del<SwitchOff>();
    }
  }
}