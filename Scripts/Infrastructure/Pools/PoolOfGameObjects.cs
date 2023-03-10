using System.Collections.Generic;
using StarGravity.Infrastructure.Factories;
using UnityEngine;

namespace StarGravity.Infrastructure.Pools
{
  public class PoolOfGameObjects
  {
    private readonly Dictionary<string, List<GameObject>> _pooled;
    private readonly IGameObjectFactory _objectFactory;

    public PoolOfGameObjects(IGameObjectFactory objectFactory)
    {
      _objectFactory = objectFactory;
      _pooled = new Dictionary<string, List<GameObject>>();
    }

    public GameObject Get(GameObject prefab, Vector2 position)
    {
      GameObject gameObject;
      if (_pooled.ContainsKey(prefab.name))
      {
        gameObject = _pooled[prefab.name].Find(x => x.activeInHierarchy == false);
        if (gameObject == null) 
          gameObject = Create(prefab, position);
        else
          ResetGameObject(position, gameObject);
      }
      else
      {
        _pooled.Add(prefab.name, new List<GameObject>());
        gameObject = Create(prefab, position); ;
      }
      
      return gameObject;
    }

    private GameObject Create(GameObject prefab, Vector2 position)
    {
      GameObject gameObject = _objectFactory.CreateStarOrPlanet(prefab, position);
      _pooled[prefab.name].Add(gameObject);
      return gameObject;
    }

    protected virtual void ResetGameObject(Vector2 position, GameObject gameObject)
    {
      gameObject.transform.position = position;
      gameObject.SetActive(true);
    }
  }
}