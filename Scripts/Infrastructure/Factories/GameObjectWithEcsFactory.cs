using System.Collections.Generic;
using Leopotam.Ecs;
using StarGravity.GamePlay.Interactables;
using StarGravity.GamePlay.Interactables.Bonuses;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Voody.UniLeo;

namespace StarGravity.Infrastructure.Factories
{
  public class GameObjectWithEcsFactory : IGameObjectFactory
  {
    private readonly IObjectResolver _container;
    private readonly List<GameObject> _flyingBonuses = new();

    public List<GameObject> Bonuses => _flyingBonuses;

    public GameObjectWithEcsFactory(IObjectResolver container)
    {
      _container = container;
    }

    public GameObject CreateStarOrPlanet(GameObject prefab, Vector3 at)
    {
      GameObject planet = _container.Instantiate(prefab, at, Quaternion.identity);
      EcsEntity starEntity = WorldHandler.GetWorld().NewEntity();
      var convertToEntity = planet.GetComponent<ConvertToEntity>();

      foreach (var component in planet.GetComponents<Component>())
      {
        if (component is IConvertToEntity entityComponent)
        {
          entityComponent.Convert(starEntity);
          GameObject.Destroy(component);
        }
      }
      convertToEntity.Set(starEntity);
      convertToEntity.setProccessed();
      return planet;
    }
    

    public GameObject CreateInteractableGameObject(GameObject prefab, Vector3 at)
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