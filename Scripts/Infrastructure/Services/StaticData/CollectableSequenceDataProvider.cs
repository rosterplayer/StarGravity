using System.Collections.Generic;
using System.Linq;
using StarGravity.Data;
using UnityEngine;

namespace StarGravity.Infrastructure.Services.StaticData
{
  public class CollectableSequenceDataProvider : ICollectableSequenceDataProvider
  {
    private const string StarSequencesPath = "StarSequences";
    private List<ForPlanetsPositions> _sequences;

    public void LoadData()
    {
      _sequences = Resources
        .LoadAll<ForPlanetsPositions>(StarSequencesPath)
        .ToList();
    }

    public List<GameObject> GetPrefabs(PositionOnScreen leftPlanet, PositionOnScreen star, PositionOnScreen rightPlanet)
    {
      var planetPositionsComparer = new PlanetPositionComparer();
      var prefabs = _sequences
        .FindAll(x => x.ForPositions.Contains(new PlanetsPosition(leftPlanet,star,rightPlanet), planetPositionsComparer));
      List<GameObject> prefabsGameObjects = new List<GameObject>(prefabs.Count);
      prefabsGameObjects.AddRange(prefabs.Select(prefab => prefab.gameObject));

      return prefabsGameObjects;
    }

    public GameObject GetPrefabByName(string name)
    {
      var forPlanetsPositions = _sequences.Find(x => x.name == name);
      return forPlanetsPositions.gameObject;
    }
  }
}