using System;
using Leopotam.Ecs;
using StarGravity.GamePlay.Background.Systems;
using StarGravity.GamePlay.Common.Components;
using StarGravity.GamePlay.Common.Systems;
using StarGravity.GamePlay.Interactables.Comet.Systems;
using StarGravity.GamePlay.Interactables.MovingObjects.Systems;
using StarGravity.GamePlay.Interactables.Spawners.Systems;
using StarGravity.GamePlay.Planets;
using StarGravity.GamePlay.Planets.Systems;
using StarGravity.GamePlay.Player.Components;
using StarGravity.GamePlay.Player.Input.Components;
using StarGravity.GamePlay.Player.Input.Systems;
using StarGravity.GamePlay.Player.Systems;
using StarGravity.GamePlay.Routes;
using StarGravity.GamePlay.Routes.Systems;
using StarGravity.GamePlay.Stars.Systems;
using StarGravity.Infrastructure.AssetManagement;
using StarGravity.Infrastructure.Factories;
using StarGravity.Infrastructure.Services.Input;
using StarGravity.Infrastructure.Services.Progress;
using StarGravity.Infrastructure.Services.Sound;
using VContainer.Unity;
using Voody.UniLeo;

namespace StarGravity.Infrastructure.ECS
{
  public sealed class EcsGameStartup: IInitializable, ITickable, IDisposable
  {
    private EcsWorld _world;
    private EcsSystems _systems;

    private readonly IInputService _inputService;
    private readonly SoundService _soundService;
    private readonly GamePrefabs _gamePrefabs;
    private readonly UFOSpawner _ufoSpawner;
    private readonly IGameLevelProgressService _gameLevelProgressService;
    private readonly PlanetSpawner _planetSpawner;
    private readonly IGameObjectFactory _gameFactory;

    public EcsGameStartup(
      IInputService inputService,
      SoundService soundService,
      GamePrefabs gamePrefabs,
      UFOSpawner ufoSpawner,
      IGameLevelProgressService gameLevelProgressService,
      PlanetSpawner planetSpawner,
      IGameObjectFactory gameFactory)
    {
      _planetSpawner = planetSpawner;
      _gamePrefabs = gamePrefabs;
      _ufoSpawner = ufoSpawner;
      _gameLevelProgressService = gameLevelProgressService;
      _soundService = soundService;
      _inputService = inputService;
      _gameFactory = gameFactory;
    }
    
    public void Initialize()
    {
      _world = new EcsWorld();
      
#if UNITY_EDITOR
      Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_world);
#endif 
      _systems = new EcsSystems(_world);

      _systems.ConvertScene();
      
      AddSystems();
      AddOneFrames();
      AddInjections();
      
      _systems.Init();
#if UNITY_EDITOR
      Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_systems);
#endif
    }

    public void Tick()
    {
      _systems.Run();
    }

    public void Dispose()
    {
      if (_systems == null)
        return;

      _systems.Destroy();
      _systems = null;
      _world.Destroy();
      _world = null;
    }

    private void AddSystems()
    {
      _systems
        .Add(new InitMoveSystem())
        .Add(new InitRotationSystem())
        .Add(new InputSystem())
        .Add(new CometSystem())
        .Add(new FlyAfterRespawnSystem())
        .Add(new ShipRotatingSystem())
        .Add(new StartLandingSystem())
        .Add(new EndLandingSystem())
        .Add(new StartHorizontalMovingSystem())
        .Add(new StartPlanetsMovingSystem())
        .Add(new MovePlanetSystem())
        .Add(new HorizontalMovingSystem())
        .Add(new RepositionSystem())
        .Add(new UfoSpawnSystem())
        .Add(new StopHorizontalMovingSystem())
        .Add(new MovingObjectsSpawnerSystem())
        .Add(new EndPlanetsMovingSystem())
        .Add(new JumpFromOrbitSystem())
        .Add(new GravityFieldSystem())
        .Add(new RotateSystem())
        .Add(new FollowSystem())
        .Add(new CorrectVelocitySystem())
        .Add(new MoveObjectSystem())
        .Add(new CheckOutOfScreenSystem())
        .Add(new GravityFieldActivationSystem())
        .Add(new ShipCrashSystem())
        .Add(new RespawnSystem())
        .Add(new PlanetsCollidersSwitchSystem())
        .Add(new DestroyObjectsSystem());
    }

    private void AddOneFrames()
    {
      _systems
        .OneFrame<JumpPressed>()
        .OneFrame<InGravityField>()
        .OneFrame<NeedRespawn>()
        .OneFrame<CrashedEvent>()
        ;
    }

    private void AddInjections()
    {
      _systems
        .Inject(_inputService)
        .Inject(_soundService)
        .Inject(_gamePrefabs)
        .Inject(_ufoSpawner)
        .Inject(_gameLevelProgressService)
        .Inject(_planetSpawner)
        .Inject(_gameFactory)
        ;
    }
  }
}