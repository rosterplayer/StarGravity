using System;
using StarGravity.Data;
using StarGravity.GamePlay.Player;
using UnityEngine;

namespace StarGravity.Infrastructure.Services.Progress
{
  public class GameLevelProgressService
  {
    private StarShip _player;

    public GameLevelProgressData LevelProgress { get; set; }

    public event Action ProgressChanged;

    public GameLevelProgressService()
    {
      LevelProgress = new GameLevelProgressData();
    }

    public void Subscribe(StarShip playerShip)
    {
      _player = playerShip;
      _player.PlanetReached += OnPlanetReached;
      _player.BonusCollected += OnBonusCollected;
      _player.ShipBonusCollected += OnShipBonusCollected;
    }

    private void OnBonusCollected(int bonusValue)
    {
      LevelProgress.PlusBonus(bonusValue);
      ProgressChanged?.Invoke();
    }

    private void OnPlanetReached(GameObject planet)
    {
      LevelProgress.PlusSystem();
      ProgressChanged?.Invoke();
    }

    private void OnShipBonusCollected()
    {
      LevelProgress.PlusShipBonus();
      ProgressChanged?.Invoke();
    }
  }
}