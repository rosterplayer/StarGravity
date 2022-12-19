using System.Collections;
using System.Collections.Generic;
using StarGravity.GamePlay.Player;
using StarGravity.Infrastructure.Factories;
using StarGravity.Infrastructure.Services.Input;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace StarGravity.GamePlay.Interactables
{
  public enum SpawnMode
  {
    Endless = 0,
    Limited = 1
  }

  public class MovingObjectSpawner : MonoBehaviour
  {
    public List<GameObject> Prefabs;
    public SpawnMode SpawnMode;
    public int MaxSpawnedObjects = 5;
    public float SpawnCooldown = 3;
    [Range(3, 6)] public float MaxSpeed = 6;
    public float Wide;

    private int _spawned;
    
    private GameObjectFactory _gameFactory;
    private IInputService _inputService;
    private StarShip _playerShip;

    [Inject]
    public void Construct(GameObjectFactory factory, IInputService inputService, PlayerShipFactory playerShipFactory)
    {
      _gameFactory = factory;
      _inputService = inputService;
      _playerShip = playerShipFactory.PlayerShip.GetComponent<StarShip>();
      _playerShip.PlanetReached += ClearSpawned;
    }

    private void Start()
    {
      StartCoroutine(EndlessSpawner());
    }

    private IEnumerator EndlessSpawner()
    {
      while (true)
      {
        yield return new WaitForSeconds(SpawnCooldown);
        SpawnObject();
      }
    }

    private void SpawnObject()
    {
      if (_inputService.Paused || (SpawnMode == SpawnMode.Limited && _spawned >= MaxSpawnedObjects))
        return;

      _spawned++;
      GameObject bonus = _gameFactory.CreateGameObject(Prefabs[Random.Range(0, Prefabs.Count)], GetRandomPosition());
      bonus.GetComponent<MovingObject>().StartMove(Random.Range(3,MaxSpeed), GetRandomDirection());
    }

    private Vector3 GetRandomPosition() => 
      transform.TransformPoint(new Vector3(Random.Range(-Wide / 2, Wide / 2), 0));

    private Vector2 GetRandomDirection() => 
      ((Vector2)transform.up + Random.insideUnitCircle).normalized;

    private void ClearSpawned(GameObject planet) => 
      _spawned = 0;
  }
}