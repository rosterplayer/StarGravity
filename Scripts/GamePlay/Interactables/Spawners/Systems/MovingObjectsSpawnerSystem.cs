using Leopotam.Ecs;
using StarGravity.GamePlay.Common.Components;
using StarGravity.GamePlay.Interactables.MovingObjects;
using StarGravity.GamePlay.Interactables.Spawners.Components;
using StarGravity.GamePlay.Planets.Components;
using StarGravity.Infrastructure.Factories;
using StarGravity.Infrastructure.Services.Input;
using UnityEngine;

namespace StarGravity.GamePlay.Interactables.Spawners.Systems
{
  public class MovingObjectsSpawnerSystem : IEcsRunSystem
  {
    private IGameObjectFactory _gameFactory;
    private IInputService _inputService;
    
    private EcsFilter<PlanetsMovingEnded> _planetsMovingEndedFilter;
    private EcsFilter<MovingObjectSpawnerComponent, TransformComponent> _spawners;

    public void Run()
    {
      ResetSpawned();
      
      if (_inputService.Paused)
        return;
      
      foreach (int i in _spawners)
      {
        ref var spawnerComponent = ref _spawners.Get1(i);
        ref var transform = ref _spawners.Get2(i);
        
        if (spawnerComponent.TimeLeftToNextSpawn > 0)
        {
          spawnerComponent.TimeLeftToNextSpawn -= Time.deltaTime;
          continue;
        }

        spawnerComponent.ResetCooldown();
        
        if (spawnerComponent.SpawnMode == SpawnMode.Limited && spawnerComponent.Spawned >= spawnerComponent.MaxSpawnedObjects)
          continue;

        Spawn(ref spawnerComponent, ref transform);
      }
    }

    private void ResetSpawned()
    {
      if (_planetsMovingEndedFilter.IsEmpty())
        return;

      foreach (int i in _spawners)
      {
        ref var spawnerComponent = ref _spawners.Get1(i);
        spawnerComponent.Spawned = 0;
      }
    }

    private void Spawn(ref MovingObjectSpawnerComponent spawnerComponent, ref TransformComponent transform)
    {
      GameObject bonus = _gameFactory.CreateInteractableGameObject(
        spawnerComponent.Prefabs[Random.Range(0, spawnerComponent.Prefabs.Count)],
        GetRandomPosition(transform.Transform, spawnerComponent.Wide)
      );

      if (bonus.TryGetComponent(out InitMoveData initMoveData))
      {
        initMoveData.Direction = GetRandomDirection(transform.Transform);
        initMoveData.Speed = Random.Range(3, spawnerComponent.MaxSpeed);
      }

      spawnerComponent.Spawned++;
    }

    private Vector3 GetRandomPosition(Transform transform, float wide) => 
      transform.TransformPoint(new Vector3(Random.Range(-wide / 2, wide / 2), 0));

    private Vector2 GetRandomDirection(Transform transform) => 
      ((Vector2)transform.up + Random.insideUnitCircle).normalized;
  }
}