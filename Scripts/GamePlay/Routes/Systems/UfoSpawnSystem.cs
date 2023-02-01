using Leopotam.Ecs;
using StarGravity.GamePlay.Planets.Components;
using StarGravity.Infrastructure.Services.Progress;

namespace StarGravity.GamePlay.Routes.Systems
{
  public class UfoSpawnSystem : IEcsRunSystem
  {
    private UFOSpawner _ufoSpawner;
    private IGameLevelProgressService _levelProgressService;
    
    private EcsFilter<PlanetsMovingEnded> _filter;
    
    public void Run()
    {
      foreach (int i in _filter)
      {
        _ufoSpawner.Spawn(_levelProgressService.LevelProgress.Systems);
      }
    }
  }
}