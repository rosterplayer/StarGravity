using UnityEngine;

namespace StarGravity.GamePlay.Player.Perks
{
  public abstract class ShipPerk : MonoBehaviour
  {
    public StarShip PlayerShip;
    
    private void Awake()
    {
      PlayerShip.UseAbility += Activate;
    }

    protected virtual void Activate()
    {
    }
  }
}