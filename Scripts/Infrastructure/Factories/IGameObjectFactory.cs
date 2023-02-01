using System.Collections.Generic;
using UnityEngine;

namespace StarGravity.Infrastructure.Factories
{
  public interface IGameObjectFactory
  {
    List<GameObject> Bonuses { get; }
    GameObject CreateStarOrPlanet(GameObject prefab, Vector3 at);
    GameObject CreateInteractableGameObject(GameObject prefab, Vector3 at);
    GameObject CreateCollectablesSequence(GameObject prefab, Vector3 at);
    void RemoveBonusFromList(GameObject bonus);
  }
}