using UnityEngine;

namespace StarGravity.GamePlay.Routes
{
  public class ShipRoutesOnBackground : RouteSpawner
  {
    public float TimeBetweenShips = 15;
    
    private float _timeLeft;

    private void Update()
    {
      _timeLeft -= Time.deltaTime;
      
      if (_timeLeft > 0)
        return;

      CreateRoute();
      _timeLeft += TimeBetweenShips;
    }
  }
}