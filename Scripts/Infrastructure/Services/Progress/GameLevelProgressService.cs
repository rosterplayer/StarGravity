using System;
using StarGravity.Data;
using StarGravity.GamePlay.Player;
using UnityEngine;

namespace StarGravity.Infrastructure.Services.Progress
{
  public class GameLevelProgressService : IGameLevelProgressService
  {
    public GameLevelProgressData LevelProgress { get; }

    public event Action ProgressChanged;

    public GameLevelProgressService()
    {
      LevelProgress = new GameLevelProgressData();
    }

    public void OnStarBonusCollected(int bonusValue)
    {
      LevelProgress.PlusBonus(bonusValue);
      ProgressChanged?.Invoke();
    }

    public void OnNewPlanetReached()
    {
      LevelProgress.PlusSystem();
      ProgressChanged?.Invoke();
    }

    public void OnShipBonusCollected()
    {
      LevelProgress.PlusShipBonus();
      ProgressChanged?.Invoke();
    }
  }
}