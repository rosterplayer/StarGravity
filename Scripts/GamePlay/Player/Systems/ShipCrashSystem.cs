using Leopotam.Ecs;
using StarGravity.GamePlay.Common.Components;
using StarGravity.GamePlay.Player.Components;
using StarGravity.Infrastructure.AssetManagement;
using StarGravity.Infrastructure.Services.Sound;
using UnityEngine;

namespace StarGravity.GamePlay.Player.Systems
{
  public class ShipCrashSystem : IEcsRunSystem
  {
    private SoundService _soundService;
    private GamePrefabs _gamePrefabs;
    
    private EcsFilter<CrashedEvent, TransformComponent, ShipState>.Exclude<Untouchable> _crashes;
    public void Run()
    {
      foreach (int i in _crashes)
      {
        ref var shipState = ref _crashes.Get3(i);
        
        if (shipState.State != PlayerState.OnFLy)
          continue;
        
        ref var entity = ref _crashes.GetEntity(i);
        ref var transform = ref _crashes.Get2(i);

        Object.Instantiate(_gamePrefabs.ShipCrashFX, transform.Transform.position, Quaternion.identity);
        _soundService.PlaySFX(SfxType.ShipCrash);
        entity.Get<NeedRespawn>();
      }
    }
  }
}