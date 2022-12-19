using System;
using StarGravity.Infrastructure.Factories;
using StarGravity.Infrastructure.Services.Sound;
using UnityEngine;
using VContainer;

namespace StarGravity.GamePlay.Interactables
{
  public enum BonusType
  {
    LittleBonus = 0,
    HeavyBonus = 1,
    HealthBonus = 2,
    MagnetBonus = 3,
  }

  public class Bonus : InteractableMovingObject
  {
    private const int CaptureSpeed = 20;

    public BonusType BonusType;
    public int Points = 1;
    
    private GameObjectFactory _objectFactory;
    private GameObject _destinationObject;
    private MovingObject _movingObjectComponent;
    private Action _onDestroyAction;
    private SoundService _soundService;

    [Inject]
    public void Construct(GameObjectFactory factory, SoundService soundService)
    {
      _soundService = soundService;
      _objectFactory = factory;
    }

    public void MoveTo(GameObject player, Action onDestroyCallback)
    {
      _destinationObject = player;
      _onDestroyAction = onDestroyCallback;
      _movingObjectComponent = GetComponent<MovingObject>();
    }

    private void Update()
    {
      if (_destinationObject == null)
        return;

      Vector3 direction = (_destinationObject.transform.position - transform.position).normalized;
      _movingObjectComponent.StartMove(CaptureSpeed, direction);
    }

    private void OnDestroy()
    {
      _onDestroyAction?.Invoke();
      _objectFactory.RemoveBonusFromList(gameObject);
    }

    protected override void OnCollisionWithPlayer()
    {
      PlaySfx();
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