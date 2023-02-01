using Leopotam.Ecs;
using StarGravity.GamePlay.Common.Components;
using StarGravity.GamePlay.Player.Components;
using StarGravity.GamePlay.Player.Input.Components;
using StarGravity.Infrastructure.Services.Input;

namespace StarGravity.GamePlay.Player.Input.Systems
{
  public sealed class InputSystem : IEcsRunSystem
  {
    private IInputService _inputService;
    
    private EcsFilter<ShipState, TransformComponent> _playerFilter;
    
    public void Run()
    {
      _inputService.Run();
      CheckInput();
    }

    private void CheckInput()
    {
      foreach (int i in _playerFilter)
      {
        ref var entity = ref _playerFilter.GetEntity(i);
        ref var shipState = ref _playerFilter.Get1(i);
        ref var transform = ref _playerFilter.Get2(i);
        StarShip ship = transform.Transform.GetComponent<StarShip>();

        if (_inputService.PressInput && shipState.State == PlayerState.OnPlanetOrbit)
          entity.Get<JumpPressed>();

        if (_inputService.PressInput && shipState.State == PlayerState.OnFLy)
          ship.OnUseAbility();
        if (_inputService.UpInput && shipState.State == PlayerState.OnFLy)
          ship.OnUpPressed();;
        if (_inputService.DownInput && shipState.State == PlayerState.OnFLy)
          ship.OnDownPressed();;
      }
    }
  }
}