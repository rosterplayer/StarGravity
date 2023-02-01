using StarGravity.GamePlay.Planets;
using StarGravity.Infrastructure.Factories;
using UnityEngine;

namespace StarGravity.Infrastructure.Pools
{
  public class PoolOfPlanets : PoolOfGameObjects
  {
    public PoolOfPlanets(IGameObjectFactory objectFactory) : base(objectFactory) {}

    protected override void ResetGameObject(Vector2 position, GameObject gameObject)
    {
      gameObject.GetComponent<ResetableChilds>().ResetAll();
      base.ResetGameObject(position, gameObject);
    }
  }
}