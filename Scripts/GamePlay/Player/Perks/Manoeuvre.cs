using DG.Tweening;
using StarGravity.Infrastructure.Services;
using StarGravity.Infrastructure.Services.Input;
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
    
    private IInputService _inputService;
    private SoundService _soundService;

    [Inject]
    public void Construct(IInputService inputService, SoundService soundService)
    {
      _soundService = soundService;
      _inputService = inputService;
    }

    protected override void OnUpdate()
    {
      if (PlayerShip.PlayerState != PlayerState.OnFLy || IsCooldown())
        return;

      if (_inputService.UpInput)
      {
        Move(-transform.right);
        LeftEngineFx.Play();
      }
      else if (_inputService.DownInput)
      {
        Move(transform.right);
        RightEngineFx.Play();
      }
    }

    private void Move(Vector3 moveDirection)
    {
      _soundService.PlaySFX(SfxType.ManoeuvreEngine);
      transform.DOMove(transform.position + moveDirection + (Vector3)Rigidbody2D.velocity / 5, 0.3f);
      SetCooldown();
    }
  }
}