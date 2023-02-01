using Leopotam.Ecs;
using StarGravity.GamePlay.Common.Components;
using StarGravity.GamePlay.Planets;
using StarGravity.GamePlay.Planets.Components;
using StarGravity.GamePlay.Player.Components;
using StarGravity.GamePlay.Utilities;
using StarGravity.Infrastructure.Services.Progress;
using UnityEngine;
using Voody.UniLeo;

namespace StarGravity.GamePlay.Player.Systems
{
  public class EndLandingSystem : IEcsRunSystem
  {
    private IGameLevelProgressService _levelProgressService;
    private EcsWorld _world;
    private EcsFilter<LandedOn, TransformComponent, ShipState, ReachedPlanet> _ships;

    public void Run()
    {
      foreach (int i in _ships)
      {
        ref var entity = ref _ships.GetEntity(i);
        ref var landedOn = ref _ships.Get1(i);
        ref var transform = ref _ships.Get2(i);
        ref var shipState = ref _ships.Get3(i);
        ref var reachedPlanet = ref _ships.Get4(i);

        transform.Transform.SetParent(landedOn.Planet.transform);
        shipState.State = PlayerState.OnPlanetOrbit;
        reachedPlanet.Planet = landedOn.Planet;

        if (isLandedOnNewPlanet(landedOn.Planet))
        {
          CreateEvents(landedOn.Planet);
          _levelProgressService.OnNewPlanetReached();
        }

        entity.Del<LandedOn>();
      }
    }

    private bool isLandedOnNewPlanet(GameObject planet)
    {
      var destination = planet.GetComponent<Destination>();
      bool reached = destination.Reached;
      destination.MakeReached();
      return !reached;
    }

    private void CreateEvents(GameObject planet)
    {
      EcsEntity newEntity = _world.NewEntity();
      newEntity.Get<PlanetsMovingStarted>();

      if (planet.GetComponentInParent<ConvertToEntity>().TryGetEcsEntity(out EcsEntity planetEntity))
      {
        ref var correctVelocityRequest = ref newEntity.Get<CorrectVelocityRequest>();
        ref var movablePlanet = ref planetEntity.Get<MovablePlanet>();
        correctVelocityRequest.CorrectionVelocity = Vector2.left * movablePlanet.Speed;
        planetEntity.Get<PlanetWithPlayer>();
      }
    }
  }
}