using StarGravity.GamePlay.Player;
using StarGravity.Infrastructure.Factories;
using UnityEngine;
using VContainer;

namespace StarGravity.GamePlay
{
  public class HorizontalMoving : MonoBehaviour
  {
    public float Speed;
    public float MovingDistance;

    private bool _isMoving;
    private Vector2 _positionOnStartMoving;
    
    private StarShip _playerShip;

    [Inject]
    public void Construct(PlayerShipFactory playerShipFactory)
    {
      _playerShip = playerShipFactory.PlayerShip.GetComponent<StarShip>();
      _playerShip.PlanetReached += StartMove;
    }

    private void Update()
    {
      if (!_isMoving)
        return;
      
      transform.Translate(Vector3.left * Speed * Time.deltaTime);

      if (Vector2.Distance(transform.position, _positionOnStartMoving) >= MovingDistance)
      {
        _isMoving = false;
      }
    }

    private void OnDestroy()
    {
      _playerShip.PlanetReached -= StartMove;
    }

    private void StartMove(GameObject planet)
    {
      _isMoving = true;
      _positionOnStartMoving = transform.position;
    }
  }
}