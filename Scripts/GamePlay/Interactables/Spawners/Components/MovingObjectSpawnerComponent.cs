using System;
using System.Collections.Generic;
using UnityEngine;

namespace StarGravity.GamePlay.Interactables.Spawners.Components
{
  [Serializable]
  public struct MovingObjectSpawnerComponent
  {
    public List<GameObject> Prefabs;
    public SpawnMode SpawnMode;
    public int MaxSpawnedObjects;
    public float SpawnCooldown;
    [Range(3, 6)] public float MaxSpeed;
    public float Wide;
    public int Spawned;
    public float TimeLeftToNextSpawn;

    public void ResetCooldown()
    {
      TimeLeftToNextSpawn = SpawnCooldown;
    }
  }
}