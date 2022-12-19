using UnityEngine;

namespace StarGravity.GamePlay.Interactables
{
  public class InteractableMovingObject : MonoBehaviour
  {
    private void OnTriggerEnter2D(Collider2D other)
    {
      if (other.CompareTag("Player"))
      {
        OnCollisionWithPlayer();
        Destroy(gameObject);
      }
    }

    protected virtual void OnCollisionWithPlayer()
    {
    }
  }
}