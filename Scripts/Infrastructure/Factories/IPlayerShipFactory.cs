using UnityEngine;

namespace StarGravity.Infrastructure.Factories
{
  public interface IPlayerShipFactory
  {
    GameObject PlayerShip { get; }
    int Create();
  }
}