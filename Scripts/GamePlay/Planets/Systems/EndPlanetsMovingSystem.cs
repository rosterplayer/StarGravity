using Leopotam.Ecs;
using StarGravity.GamePlay.Planets.Components;
using StarGravity.Infrastructure.Services.Input;
using UnityEngine;

namespace StarGravity.GamePlay.Planets.Systems
{
  public sealed class EndPlanetsMovingSystem : IEcsRunSystem
  {
    private IInputService _inputService;
    private EcsWorld _world;
    private EcsFilter<PlanetsMovingEnded> _filter;
    private EcsFilter<PlanetWithPlayer, MovablePlanet> _planets;

    public void Run()
    {
      if (_filter.IsEmpty())
        return;
      
      RequestVelocityCorrection();

      _inputService.GainControl();
      DeleteEvent();
    }

    private void RequestVelocityCorrection()
    {
      ref var correctVelocityRequest = ref _world.NewEntity().Get<CorrectVelocityRequest>();

      float planetSpeed = 0;
      foreach (int i in _planets)
      {
        ref var movablePlanet = ref _planets.Get2(i);
        planetSpeed = movablePlanet.Speed;
        _planets.GetEntity(i).Del<PlanetWithPlayer>();
      }

      correctVelocityRequest.CorrectionVelocity = Vector2.right * planetSpeed;
    }

    private void DeleteEvent()
    {
      foreach (int i in _filter)
      {
        _filter.GetEntity(i).Del<PlanetsMovingEnded>();
      }
    }
  }
}