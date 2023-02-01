using UnityEngine;
using Random = UnityEngine.Random;

namespace StarGravity.Infrastructure.AssetManagement
{
  [CreateAssetMenu(fileName = "GamePrefabs", menuName = "Assets/GamePrefabs", order = 0)]
  public class GamePrefabs : ScriptableObject
  {
    [Header("Gameplay")]
    public GameObject[] PlayerStarships;
    public GameObject[] Planets;
    public GameObject[] Stars;
    public GameObject CaptureBeam;

    [Header("VFX")] 
    public GameObject ShipCrashFX;
    public GameObject AsteroidCrashFX;

    [Header("UI")]
    public GameObject GameOverPopup;

    [Header("Tutorials")]
    public GameObject[] TutorialPrefabs;
    
    [Header("Main menu")]
    public GameObject AuthPopup;

    public GameObject GetRandomPlanetPrefab() => 
      Planets[Random.Range(0, Planets.Length)];
    
    public GameObject GetRandomStarPrefab() => 
      Stars[Random.Range(0, Stars.Length)];
  }
}