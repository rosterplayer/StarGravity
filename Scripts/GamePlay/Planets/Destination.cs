using DG.Tweening;
using UnityEngine;
using VContainer;

namespace StarGravity.GamePlay.Planets
{
  public class Destination : MonoBehaviour, IResetable
  {
    [SerializeField] private Collider2D _collider;
    
    private bool _reached;
    private PlanetSpawner _planetSpawner;

    public bool Reached => _reached;

    [Inject]
    public void Constructor(PlanetSpawner planetSpawner)
    {
      _planetSpawner = planetSpawner;
    }

    public void MakeReached()
    {
      _reached = true;
    }

    public void Reset()
    {
      _reached = false;
    }

    public void SwitchOnColliderWithDelay()
    {
      DOVirtual.DelayedCall(0.5f, SwitchOnCollider);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
      if (!col.collider.CompareTag("Player"))
        return;
      
      SwitchOffCollider();

      if (!_reached)
      {
        //MakeReached();
        _planetSpawner.SpawnNext();
      }
    }

    private void SwitchOnCollider() => _collider.enabled = true;

    private void SwitchOffCollider() => _collider.enabled = false;
  }
}