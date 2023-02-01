using DG.Tweening;
using StarGravity.Infrastructure.Services;
using StarGravity.Infrastructure.Services.Sound;
using UnityEngine;
using VContainer;

namespace StarGravity.GamePlay.Player.Perks
{
  public class Manoeuvre : ShipPerkWithCooldown
  {
    public Rigidbody2D Rigidbody2D;
    public ParticleSystem RightEngineFx;
    public ParticleSystem LeftEngineFx;

    private SoundService _soundService;

    [Inject]
    public void Construct(SoundService soundService)
    {
      _soundService = soundService;
    }

    private void Awake()
    {
      PlayerShip.UpPressed += OnUpPressed;
      PlayerShip.DownPressed += OnDownPressed;
    }

    private void OnUpPressed()
    {
      if (IsCooldown())
        return;
      
      Move(-transform.right);
      LeftEngineFx.Play();
    }
    
    private void OnDownPressed()
    {
      if (IsCooldown())
        return;
      
      Move(transform.right);
      RightEngineFx.Play();
    }

    private void Move(Vector3 moveDirection)
    {
      _soundService.PlaySFX(SfxType.ManoeuvreEngine);
      transform.DOMove(transform.position + moveDirection + (Vector3)Rigidbody2D.velocity / 5, 0.3f);
      SetCooldown();
    }
  }
}