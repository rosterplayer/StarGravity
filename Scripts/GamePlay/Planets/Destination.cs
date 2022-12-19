using DG.Tweening;
using UnityEngine;
using VContainer;

namespace StarGravity.GamePlay.Planets
{
  public class Destination : MonoBehaviour
  {
    private bool _reached;
    private PlanetSpawner _planetSpawner;
    private Collider2D _collider;
    
    public bool Reached => _reached;

    [Inject]
    public void Constructor(PlanetSpawner planetSpawner)
    {
      _planetSpawner = planetSpawner;
    }

    private void Awake()
    {
      _collider = GetComponent<Collider2D>();
    }

    public void MakeReached()
    {
      _reached = true;
    }

    public void SwitchOnCollider() => _collider.enabled = true;
    public void SwitchOffCollider() => _collider.enabled = false;

    public void SwitchOnColliderWithDelay()
    {
      DOVirtual.DelayedCall(0.5f, SwitchOnCollider);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
      if (_reached || !col.collider.CompareTag("Player"))
        return;
      
      _planetSpawner.SpawnNext();
    }
  }
}