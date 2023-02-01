using System;
using Leopotam.Ecs;
using StarGravity.GamePlay.Planets;
using StarGravity.GamePlay.Player;
using StarGravity.GamePlay.Player.Components;
using StarGravity.GamePlay.Utilities;
using StarGravity.Infrastructure.Factories;
using StarGravity.Infrastructure.Services.Ad;
using StarGravity.Infrastructure.Services.Input;
using StarGravity.Infrastructure.Services.Progress;
using StarGravity.Infrastructure.Services.SDK;
using StarGravity.Infrastructure.Services.Sound;
using StarGravity.Infrastructure.Services.Tutorial;
using VContainer.Unity;
using Voody.UniLeo;
using Health = StarGravity.GamePlay.Player.Health;

namespace StarGravity.GamePlay
{
  public class Game : IInitializable, IStartable, IDisposable
  {
    private readonly PlanetSpawner _planetSpawner;
    private readonly IPlayerShipFactory _playerShipFactory;
    private readonly SoundService _soundService;
    private readonly ISDKWrapper _sdk;
    private readonly IInputService _inputService;
    private readonly IAdService _adService;
    private readonly IGameLevelProgressService _levelProgressService;
    private readonly TutorialService _tutorial;

    private Health _health;
    private int _shipNumber;

    public Game(
      PlanetSpawner planetSpawner,
      IPlayerShipFactory playerFactory,
      SoundService soundService,
      ISDKWrapper sdk,
      IInputService inputService,
      IAdService adService,
      IGameLevelProgressService levelProgressService,
      TutorialService tutorial
      )
    {
      _planetSpawner = planetSpawner;
      _playerShipFactory = playerFactory;
      _soundService = soundService;
      _sdk = sdk;
      _inputService = inputService;
      _adService = adService;
      _levelProgressService = levelProgressService;
      _tutorial = tutorial;
    }

    public void Initialize()
    {
      SpawnShipAndPlanets();
      _adService.DecreaseRewardedAdBonusCounter();
    }

    public void Start()
    {
      _soundService.PlayMusic(SceneType.Game);
      _tutorial.ShowTutorial(_shipNumber);
      _inputService.GainControl();
    }

    private void SpawnShipAndPlanets()
    {
      _shipNumber = _playerShipFactory.Create();
      StarShip starShip = _playerShipFactory.PlayerShip.GetComponent<StarShip>();
      if (starShip.GetComponent<ConvertToEntity>().TryGetEcsEntity(out EcsEntity entity))
      {
        ref var reachedPlanet = ref entity.Get<ReachedPlanet>();
        reachedPlanet.Planet = _planetSpawner.FirstSpawn();
      }

      _planetSpawner.SpawnNext();
      
      _health = _playerShipFactory.PlayerShip.GetComponent<Health>();
      _health.OnDied += GameOver;
    }

    private void GameOver()
    {
      _sdk.SaveScore(_levelProgressService.LevelProgress.TotalPoints, _levelProgressService.LevelProgress.BonusStars);
      
      _inputService.ReleaseControl();
      
      _soundService.StopMusic();
      _soundService.PlaySFX(SfxType.Lose);
      
      _adService.ShowAd();
    }

    public void Dispose()
    {
      _health.OnDied -= GameOver;
    }
  }
}