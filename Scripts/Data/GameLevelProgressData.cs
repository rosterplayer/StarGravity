using System;

namespace StarGravity.Data
{
  [Serializable]
  public class GameLevelProgressData
  {
    public int Systems;
    public int BonusStars;
    public int ShipBonusesCollected;
    public int TotalPoints;

    public GameLevelProgressData()
    {
      Systems = 1;
      BonusStars = 0;
      TotalPoints = 0;
      ShipBonusesCollected = 0;
    }

    public void PlusSystem()
    {
      Systems++;
      TotalPoints += 50;
    }

    public void PlusBonus(int value)
    {
      BonusStars += value;
      TotalPoints += value;
    }

    public void PlusShipBonus()
    {
      ShipBonusesCollected++;
      TotalPoints += 5;
    }
  }
}