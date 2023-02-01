using System;
using StarGravity.Data;

namespace StarGravity.Infrastructure.Services.Progress
{
  public interface IGameLevelProgressService
  {
    GameLevelProgressData LevelProgress { get; }
    event Action ProgressChanged;
    void OnStarBonusCollected(int bonusValue);
    void OnNewPlanetReached();
    void OnShipBonusCollected();
  }
}