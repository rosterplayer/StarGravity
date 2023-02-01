using StarGravity.GamePlay.Interactables.MovingObjects;
using StarGravity.Infrastructure.Services.Sound;
using UnityEngine;
using VContainer;

namespace StarGravity.GamePlay.Interactables.Asteroids
{
  public class Asteroid : InteractableMovingObject
  {
    public GameObject ExplosionPrefab;
    
    private SoundService _soundService;

    [Inject]
    public void Construct(SoundService soundService)
    {
      _soundService = soundService;
    }

    protected override void OnCollisionWithPlayer()
    {
      Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
      _soundService.PlaySFX(SfxType.AsteroidCrash);
      MarkForDestroy();
    }
  }
}