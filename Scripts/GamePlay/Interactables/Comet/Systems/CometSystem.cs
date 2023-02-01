using Leopotam.Ecs;
using StarGravity.GamePlay.Common.Components;
using StarGravity.GamePlay.Interactables.Comet.Components;
using UnityEngine;

namespace StarGravity.GamePlay.Interactables.Comet.Systems
{
  public class CometSystem : IEcsRunSystem, IEcsInitSystem
  {
    private EcsFilter<CometComponent, TransformComponent> _comets;
    
    private Camera _mainCamera;
    private Rect _rect = new(-1f, -1f, 3f, 3f);

    public void Init()
    {
      _mainCamera = Camera.main;

      foreach (int i in _comets)
      {
        ref var comet = ref _comets.Get1(i);
        ref var transform = ref _comets.Get2(i);

        if (!comet.Randomize)
        {
          comet.CurrentSpawnTime = comet.SpawnTime;
          comet.CurrentSpeed = comet.Speed;
        }

        Generate(ref comet, ref transform);
      }
    }

    public void Run()
    {
      foreach (int i in _comets)
      {
        UpdateComet(i);
      }
    }

    private void UpdateComet(int i)
    {
      ref var comet = ref _comets.Get1(i);
      ref var transform = ref _comets.Get2(i);

      if (comet.IsActivated)
      {
        transform.Transform.position += transform.Transform.forward * comet.CurrentSpeed * Time.deltaTime;
        //Ask if the comet has reached the limit to be regenerated
        if (!_rect.Contains(_mainCamera.WorldToViewportPoint(transform.Transform.position)))
        {
          Generate(ref comet, ref transform);
        }
      }
      else
      {
        comet.CurrentSpawnTime -= Time.deltaTime;

        if (comet.CurrentSpawnTime <= 0)
          Activate(ref comet, ref transform, true);
      }
    }

    private void Generate(ref CometComponent comet, ref TransformComponent transformComponent)
    {
      //Deactivate the comet
      Activate(ref comet, ref transformComponent, false);
      //Randomize spawn time and speed
      if (comet.Randomize)
      {
        comet.CurrentSpawnTime = Random.Range(0.3f, comet.SpawnTime);
        comet.CurrentSpeed = Random.Range(35f, comet.Speed);
      }
    }

    private void Activate(ref CometComponent comet, ref TransformComponent transform, bool activate)
    {
      comet.IsActivated = activate;
      if (comet.IsActivated)
      {
        //Once activated, the first action is to give the comet a new position
        Vector3 newPosition = _mainCamera.ViewportToWorldPoint(new Vector3(Random.Range(-0.5f, 1f), (Random.Range(0, 100) < 50 ? -0.5f : 1.5f), 0f));
            
        transform.Transform.position = new Vector3(newPosition.x, newPosition.y, transform.Transform.position.z);
        //It defines the point to the comet will be pointing
        Vector3 forwardDirection = new Vector3(_mainCamera.transform.position.x, _mainCamera.transform.position.y, 0f) - transform.Transform.position;
        transform.Transform.forward = new Vector3(forwardDirection.x, forwardDirection.y, 0f);
      }
    }
  }
}