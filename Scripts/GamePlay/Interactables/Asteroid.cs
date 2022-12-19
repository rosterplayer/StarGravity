using StarGravity.Infrastructure.Services.Sound;
using UnityEngine;
using VContainer;

namespace StarGravity.GamePlay.Interactables
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

    private void OnTriggerEnter(Collider other)
    {
      if (other.CompareTag("Shield"))
      {
        OnCollisionWithPlayer();
        Destroy(gameObject);
      }
    }

    protected override void OnCollisionWithPlayer()
    {
      Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
      _soundService.PlaySFX(SfxType.AsteroidCrash);
    }
  }
}