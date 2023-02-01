using System;
using UnityEngine;

namespace StarGravity.GamePlay.Interactables.Comet.Components
{
  [Serializable]
  public struct CometComponent
  {
    //Time that takes in activate the comet after generation
    [Range(0.3f, 30.0f)] public float SpawnTime;

    //This speed value to move the comet
    [Range(35f, 150f)] public float Speed;

    //Define if the spawnTime and the speed should be randomized or not at generation
    public bool Randomize;

    //If not activated, the comet does not move
    [HideInInspector] public bool IsActivated;
    [HideInInspector] public float CurrentSpawnTime;
    [HideInInspector] public float CurrentSpeed;
  }
}