using System;
using StarGravity.GamePlay.Planets;
using StarGravity.GamePlay.Player;
using StarGravity.GamePlay.Routes;
using StarGravity.Infrastructure.Factories;
using StarGravity.Infrastructure.Services.Ad;
using StarGravity.Infrastructure.Services.Input;
using StarGravity.Infrastructure.Services.Progress;
using StarGravity.Infrastructure.Services.SDK;
using StarGravity.Infrastructure.Services.Sound;
using StarGravity.Infrastructure.Services.Tutorial;
using UnityEngine;
using VContainer.Unity;

namespace StarGravity.GamePlay
{
  public class Game : IInitializable, IStartable, IDisposable
  {
    private readonly PlanetSpawner _planetSpawner;
    private readonly PlayerShipFactory _playerShipFactory;
    private readonly SoundService _soundService;
    private readonly ISDKWrapper _sdk;
    private readonly IInputService _inputService;
    private readonly AdService _adService;
    private readonly UFOSpawner _ufoSpawner;
    private readonly GameLevelProgressService _levelProgressService;
    private readonly TutorialService _tutorial;

    private Health _health;
    private PlanetMove _planetMove;
    private int _ship;

    public Game(
      PlanetSpawner planetSpawner,
      PlayerShipFactory playerFactory,
      SoundService soundService,
      ISDKWrapper sdk,
      IInputService inputService,
      AdService adService,
      UFOSpawner ufoSpawner,
      GameLevelProgressService levelProgressService,
      TutorialService tutorial
      )
    {
      _planetSpawner = planetSpawner;
      _playerShipFactory = playerFactory;
      _soundService = soundService;
      _sdk = sdk;
      _inputService = inputService;
      _adService = adService;
      _ufoSpawner = ufoSpawner;
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
      _tutorial.ShowTutorial(_ship);
    }

    private void SpawnShipAndPlanets()
    {
      _ship = _playerShipFactory.SpawnPlayerShip();
      StarShip starShip = _playerShipFactory.PlayerShip.GetComponent<StarShip>();
      starShip.Initialize(_planetSpawner.FirstSpawn());
      starShip.PlanetReached += SetUfoEvent;
      
      _planetSpawner.SpawnNext();
      
      _levelProgressService.Subscribe(starShip);

      _health = _playerShipFactory.PlayerShip.GetComponent<Health>();
      _health.OnDied += GameOver;
    }

    private void SetUfoEvent(GameObject planet)
    {
      // Call UFO spawn
      _planetMove = planet.GetComponentInParent<PlanetMove>();
      _planetMove.OnStopMoving += SpawnUfo;
    }

    private void SpawnUfo()
    {
      _ufoSpawner.Spawn(_levelProgressService.LevelProgress.Systems);
      _planetMove.OnStopMoving -= SpawnUfo;
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