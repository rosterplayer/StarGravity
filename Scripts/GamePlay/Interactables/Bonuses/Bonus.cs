using System;
using Leopotam.Ecs;
using StarGravity.GamePlay.Interactables.MovingObjects;
using StarGravity.GamePlay.Interactables.MovingObjects.Components;
using StarGravity.GamePlay.Utilities;
using StarGravity.Infrastructure.Factories;
using StarGravity.Infrastructure.Services.Sound;
using UnityEngine;
using VContainer;

namespace StarGravity.GamePlay.Interactables.Bonuses
{
  public class Bonus : InteractableMovingObject
  {
    public BonusType BonusType;
    public int Points = 1;
    
    private IGameObjectFactory _objectFactory;
    private SoundService _soundService;
    
    private Action _onDestroyAction;

    [Inject]
    public void Construct(IGameObjectFactory factory, SoundService soundService)
    {
      _soundService = soundService;
      _objectFactory = factory;
    }

    public void MoveTo(GameObject player, Action onDestroyCallback)
    {
      if (!_convertToEntity.TryGetEcsEntity(out EcsEntity entity))
        return;
      
      ref var followComponent = ref entity.Get<FollowComponent>();

      followComponent.FollowTo = player.transform;
      _onDestroyAction = onDestroyCallback;
    }

    protected override void OnCollisionWithPlayer()
    {
      PlaySfx();
      MarkForDestroy();
    }

    private void OnDestroy()
    {
      _onDestroyAction?.Invoke();
      _objectFactory.RemoveBonusFromList(gameObject);
    }

    private void PlaySfx()
    {
      switch (BonusType)
      {
        case BonusType.LittleBonus:
          _soundService.PlaySFX(SfxType.Bonus);
          break;
        case BonusType.HeavyBonus:
          _soundService.PlaySFX(SfxType.HeavyBonus);
          break;
        case BonusType.HealthBonus:
          _soundService.PlaySFX(SfxType.HPBonus);
          break;
        case BonusType.MagnetBonus:
          _soundService.PlaySFX(SfxType.HPBonus);
          break;
      }
    }
  }
}