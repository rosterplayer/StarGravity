using System;
using StarGravity.GamePlay.Player;
using StarGravity.Infrastructure.Factories;
using StarGravity.Infrastructure.Services.Input;
using UnityEngine;
using VContainer;

namespace StarGravity.GamePlay
{
  public class PlanetMove : MonoBehaviour
  {
    public float Speed;

    private MovePoints _movePoints;
    private bool _isMoving;
    private Vector2 _destinationPosition;

    private StarShip _playerShip;
    private IInputService _inputService;

    public event Action OnStopMoving;

    [Inject]
    public void Construct(PlayerShipFactory playerShipFactory, IInputService inputService)
    {
      _playerShip = playerShipFactory.PlayerShip.GetComponent<StarShip>();
      _inputService = inputService;
    }

    public void SetComponent(MovePoints movePoints)
    {
      _movePoints = movePoints;
      _playerShip.PlanetReached += StartMove;
    }

    public void ResetComponent()
    {
      _movePoints = null;
      _isMoving = false;
      _playerShip.PlanetReached -= StartMove;
    }

    private void Update()
    {
      if (!_isMoving)
        return;
      
      transform.Translate(Vector3.left * Speed * Time.deltaTime);

      if (transform.position.x <= _destinationPosition.x)
      {
        _isMoving = false;
        _inputService.GainControl();
        OnStopMoving?.Invoke();
      }
    }

    private void OnDestroy()
    {
      _playerShip.PlanetReached -= StartMove;
    }

    private void StartMove(GameObject planet)
    {
      float? destinationPoint = _movePoints.GetNextPointX();
      
      if (destinationPoint == null)
        return;

      _destinationPosition = new Vector2((float)destinationPoint, transform.position.y);
      _inputService.ReleaseControl();
      _isMoving = true;
    }
  }
}