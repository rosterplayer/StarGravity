using System;
using StarGravity.GamePlay.Player;
using StarGravity.Infrastructure.Factories;
using UnityEngine;
using VContainer;

namespace StarGravity.GamePlay.Interactables
{
  public class MovingObject : MonoBehaviour
  {
    private const int Offset = 10;
    
    public float Speed;
    public Vector2 Direction;
    
    private StarShip _playerShip;
    private Vector2 _oldDirection;
    private float _oldSpeed;
    private PlanetMove _planetHorizontalMoving;
    private Vector2 _bottomLeftPoint;
    private Vector2 _topRightPoint;

    [Inject]
    public void Construct(PlayerShipFactory playerShipFactory)
    {
      _playerShip = playerShipFactory.PlayerShip.GetComponent<StarShip>();
      _playerShip.PlanetReached += AdjustDirectionAndSpeed;
    }

    public void StartMove(float speed, Vector2 direction)
    {
      Speed = speed;
      Direction = direction;
    }

    private void Awake()
    {
      _bottomLeftPoint = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
      _topRightPoint = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
    }

    private void Update()
    {
      transform.Translate(Direction * (Speed * Time.deltaTime));
      CheckOutOfCamera();
    }

    private void CheckOutOfCamera()
    {
      if (transform.position.x > _topRightPoint.x + Offset * 4
          || transform.position.y > _topRightPoint.y + Offset
          || transform.position.x < _bottomLeftPoint.x - Offset
          || transform.position.y < _bottomLeftPoint.y - Offset)
        Destroy(gameObject);
    }

    private void AdjustDirectionAndSpeed(GameObject planet)
    {
      _oldDirection = Direction;
      _oldSpeed = Speed;
      _planetHorizontalMoving = planet.GetComponentInParent<PlanetMove>();
      Vector2 newDirection = Direction * Speed + Vector2.left * _planetHorizontalMoving.Speed;
      Direction = newDirection.normalized;
      Speed = newDirection.magnitude;
      _planetHorizontalMoving.OnStopMoving += ReturnNormalDirectionAndSpeed;
    }

    private void ReturnNormalDirectionAndSpeed()
    {
      Direction = _oldDirection;
      Speed = _oldSpeed;
    }

    private void OnDestroy()
    {
      if (_planetHorizontalMoving != null)
        _planetHorizontalMoving.OnStopMoving -= ReturnNormalDirectionAndSpeed;
    }
  }
}