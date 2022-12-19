using UnityEngine;

namespace StarGravity.GamePlay.Routes
{
  public class UFOSpawner : RouteSpawner
  {
    [SerializeField]
    private int _nextSpawn;

    public void Spawn(int spawnDelay)
    {
      if (_nextSpawn < spawnDelay)
      {
        SetNextSpawn();
        return;
      }

      if (_nextSpawn == spawnDelay)
      {
        CreateRoute();
        SetNextSpawn();
      }
    }

    private void SetNextSpawn()
    {
      _nextSpawn += 10 + Random.Range(0, 6);
    }
  }
}