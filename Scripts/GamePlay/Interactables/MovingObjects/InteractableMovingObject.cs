using Leopotam.Ecs;
using StarGravity.GamePlay.Common.Components;
using StarGravity.GamePlay.Utilities;
using UnityEngine;
using Voody.UniLeo;

namespace StarGravity.GamePlay.Interactables.MovingObjects
{
  public abstract class InteractableMovingObject : MonoBehaviour
  {
    [SerializeField] protected ConvertToEntity _convertToEntity;

    private void OnTriggerEnter2D(Collider2D other)
    {
      if (other.CompareTag("Player"))
      {
        OnCollisionWithPlayer();
      }
    }

    protected abstract void OnCollisionWithPlayer();

    protected void MarkForDestroy()
    {
      if (_convertToEntity.TryGetEcsEntity(out EcsEntity entity))
      {
        entity.Get<ForDestroy>();
      }
    }
  }
}