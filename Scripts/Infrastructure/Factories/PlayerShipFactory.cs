using System;
using JetBrains.Annotations;
using StarGravity.Data;
using StarGravity.Infrastructure.AssetManagement;
using StarGravity.Infrastructure.Services;
using StarGravity.Infrastructure.Services.Progress;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace StarGravity.Infrastructure.Factories
{
  [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
  public class PlayerShipFactory
  {
    private static readonly Vector3 SpawnPoint = new(-20, 0, 0);
    
    private readonly GameObject[] _shipPrefabs;
    private readonly ProgressService _progress;
    private readonly IObjectResolver _container;
    
    private GameObject _playerShip;

    public GameObject PlayerShip => _playerShip;

    public PlayerShipFactory(GamePrefabs gamePrefabs, ProgressService progress, IObjectResolver container)
    {
      _shipPrefabs = gamePrefabs.PlayerStarships;
      _progress = progress;
      _container = container;
    }

    public int SpawnPlayerShip()
    {
      if (_playerShip != null)
        Object.Destroy(_playerShip);
      
      string userDataSkin = _progress.UserData.Skin;
      if (string.IsNullOrEmpty(userDataSkin) || !Enum.TryParse(userDataSkin, out Skins skin))
      {
        _playerShip = _container.Instantiate(_shipPrefabs[0], SpawnPoint, Quaternion.identity);
        return 0;
      }

      _playerShip = _container.Instantiate(_shipPrefabs[(int)skin], SpawnPoint, Quaternion.identity);
      return (int)skin;
    }
  }
}