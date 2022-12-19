using System.Collections.Generic;
using StarGravity.Data;
using UnityEngine;

namespace StarGravity.Infrastructure.Services.StaticData
{
  public interface ICollectableSequenceDataProvider
  {
    void LoadData();
    List<GameObject> GetPrefabs(PositionOnScreen leftPlanet, PositionOnScreen star, PositionOnScreen rightPlanet);
    GameObject GetPrefabByName(string name);
  }
}