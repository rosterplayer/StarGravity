using Leopotam.Ecs;
using StarGravity.GamePlay.Common.Components;
using StarGravity.GamePlay.Planets.Components;
using StarGravity.GamePlay.Utilities;
using StarGravity.Infrastructure.AssetManagement;
using StarGravity.Infrastructure.Factories;
using StarGravity.Infrastructure.Pools;
using StarGravity.Infrastructure.Services.StaticData;
using UnityEngine;
using Voody.UniLeo;

namespace StarGravity.GamePlay.Planets
{
  public class PlanetSpawner
  {
    private readonly Vector2 _firstPlanet = new(-11.5f,0);
    private readonly Vector2 _secondPlanet = new(11.5f,0);
    private readonly Vector2 _star = new(0,0);
    private readonly Vector2 _outside = new(-40,0);

    private float _distance;
    private GameObject _currentLeftPlanet;
    private GameObject _currentRightPlanet;
    private int _spawnCount;

    private readonly IGameObjectFactory _factory;
    private readonly GamePrefabs _prefabs;
    private readonly SequenceElector _sequenceElector;
    private readonly PoolOfGameObjects _poolOfStars;
    private readonly PoolOfPlanets _poolOfPlanets;
    
    public PlanetSpawner(IGameObjectFactory factory, GamePrefabs prefabs, ICollectableSequenceDataProvider sequenceDataProvider)
    {
      _factory = factory;
      _prefabs = prefabs;
      _poolOfStars = new PoolOfGameObjects(_factory);
      _poolOfPlanets = new PoolOfPlanets(_factory);
      _sequenceElector = new SequenceElector(sequenceDataProvider);
    }

    public GameObject FirstSpawn()
    {
      _distance = Vector2.Distance(_firstPlanet, _star);
      GameObject firstPlanet = SpawnReachedPlanet(_firstPlanet, new MovePoints(new []{_outside.x}));
      GameObject secondPlanet = SpawnPlanet(_secondPlanet, new MovePoints(new []{_firstPlanet.x, _outside.x}));
      _currentLeftPlanet = firstPlanet;
      _currentRightPlanet = secondPlanet;
      SpawnStar(_prefabs.Stars[1], _star, new MovePoints(new []{_outside.x}));
      return firstPlanet;
    }

    public void SpawnNext()
    {
      Vector2 lastPlanetPosition = new Vector2(_secondPlanet.x + 2 * _distance * Mathf.Clamp(_spawnCount++, 0, 1), _secondPlanet.y);
      GameObject nextPlanet = SpawnPlanet(lastPlanetPosition + new Vector2(_distance * 2f, 0), CreateMovePoints(lastPlanetPosition.x, 4));
      _currentLeftPlanet = _currentRightPlanet;
      _currentRightPlanet = nextPlanet;
      SpawnStar(lastPlanetPosition + new Vector2(_distance, 0), CreateMovePoints(lastPlanetPosition.x - _distance, _spawnCount == 1 ? 2 : 3));
    }

    private void SpawnStar(Vector2 at, MovePoints movePoints)
    {
      GameObject randomStarPrefab = _prefabs.GetRandomStarPrefab();
      SpawnStar(randomStarPrefab, at, movePoints);
    }

    private void SpawnStar(GameObject prefab, Vector2 at, MovePoints movePoints)
    {
      GameObject planet = _poolOfStars.Get(prefab, new Vector3(at.x, RandomYForStar()));
      SetEcsComponent(planet, movePoints);
      SpawnCollectables(planet.transform.position);
    }

    private GameObject SpawnPlanet(Vector2 at, MovePoints movePoints)
    {
      GameObject spawnPlanet = _poolOfPlanets.Get(_prefabs.GetRandomPlanetPrefab(), new Vector3(at.x, RandomYForPlanet()));
      SetEcsComponent(spawnPlanet, movePoints);
      return spawnPlanet;
    }

    private GameObject SpawnReachedPlanet(Vector2 at, MovePoints movePoints)
    {
      GameObject planet = SpawnPlanet(at, movePoints);
      planet.GetComponentInChildren<Destination>().MakeReached();
      return planet;
    }

    private void SpawnCollectables(Vector2 starPosition)
    {
      var collectablePrefab = _sequenceElector.Elect(_currentLeftPlanet.transform.position, starPosition,_currentRightPlanet.transform.position);
      _factory.CreateCollectablesSequence(collectablePrefab, starPosition);
    }

    private static void SetEcsComponent(GameObject planet, MovePoints movePoints)
    {
      if (planet.GetComponent<ConvertToEntity>().TryGetEcsEntity(out EcsEntity entity))
      {
        ref var movablePlanet = ref entity.Get<MovablePlanet>();
        movablePlanet.MovePoints = movePoints;
        entity.Del<NotActive>();
        entity.Del<IsMoving>();
      }
    }

    private MovePoints CreateMovePoints(float originX, int length)
    {
      var points = new float[length];
      for (int i = 0; i < length - 1; i++)
      {
        points[i] = originX - 2 * i * _distance;
      }

      points[length - 1] = _outside.x;
      return new MovePoints(points);
    }

    private static float RandomYForPlanet() => 
      Random.Range(-4.5f, 4.5f);
    
    private static float RandomYForStar() => 
      Random.Range(-4.2f, 4.2f);
  }
}