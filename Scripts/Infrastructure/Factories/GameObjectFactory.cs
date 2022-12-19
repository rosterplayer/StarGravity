using System.Collections.Generic;
using StarGravity.GamePlay.Interactables;
using StarGravity.GamePlay.Planets;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace StarGravity.Infrastructure.Factories
{
  public class GameObjectFactory
  {
    private readonly IObjectResolver _container;
    private readonly List<GameObject> _flyingBonuses = new();

    public List<GameObject> Bonuses => _flyingBonuses;

    public GameObjectFactory(IObjectResolver container)
    {
      _container = container;
    }

    public GameObject CreatePlanet(GameObject prefab, Vector3 at) => 
      _container.Instantiate(prefab, at, Quaternion.identity);

    public GameObject CreateReachedPlanet(GameObject prefab, Vector3 at)
    {
      GameObject go = CreatePlanet(prefab, at);
      go.GetComponentInChildren<Destination>().MakeReached();
      return go;
    }

    public GameObject CreateGameObject(GameObject prefab, Vector3 at)
    {
      GameObject gameObject = _container.Instantiate(prefab, at, Quaternion.identity);
      if (gameObject.TryGetComponent(out Bonus _))
        _flyingBonuses.Add(gameObject);
      
      return gameObject;
    }

    public GameObject CreateCollectablesSequence(GameObject prefab, Vector3 at)
    {
      GameObject gameObject = _container.Instantiate(prefab, at, Quaternion.identity);
      Bonus[] bonuses = gameObject.GetComponentsInChildren<Bonus>();
      foreach (Bonus bonus in bonuses)
      {
        _flyingBonuses.Add(bonus.gameObject);
      }
      
      return gameObject;
    }

    public void RemoveBonusFromList(GameObject bonus) => 
      _flyingBonuses.Remove(bonus);
  }
}