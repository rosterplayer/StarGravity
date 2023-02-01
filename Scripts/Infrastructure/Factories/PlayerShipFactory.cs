using System;
using JetBrains.Annotations;
using Leopotam.Ecs;
using StarGravity.Data;
using StarGravity.GamePlay.Player;
using StarGravity.GamePlay.Player.Components;
using StarGravity.GamePlay.Utilities;
using StarGravity.Infrastructure.AssetManagement;
using StarGravity.Infrastructure.Services;
using StarGravity.Infrastructure.Services.Progress;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Voody.UniLeo;
using Object = UnityEngine.Object;

namespace StarGravity.Infrastructure.Factories
{
  [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
  public class PlayerShipFactory : IPlayerShipFactory
  {
    private static readonly Vector3 SpawnPoint = new(-20, 0, 0);
    
    private readonly GameObject[] _shipPrefabs;
    private readonly IProgressService _progress;
    private readonly IObjectResolver _container;
    
    private GameObject _playerShip;

    public GameObject PlayerShip => _playerShip;

    public PlayerShipFactory(GamePrefabs gamePrefabs, IProgressService progress, IObjectResolver container)
    {
      _shipPrefabs = gamePrefabs.PlayerStarships;
      _progress = progress;
      _container = container;
    }

    public int Create()
    {
      if (_playerShip != null)
      {
        Object.Destroy(_playerShip);
        if (_playerShip.GetComponent<ConvertToEntity>().TryGetEcsEntity(out EcsEntity entity))
        {
          entity.Destroy();
        }
      }
      
      string userDataSkin = _progress.UserData.Skin;
      if (string.IsNullOrEmpty(userDataSkin) || !Enum.TryParse(userDataSkin, out Skins skin))
      {
        _playerShip = _container.Instantiate(_shipPrefabs[0], SpawnPoint, Quaternion.identity);
        return 0;
      }

      _playerShip = _container.Instantiate(_shipPrefabs[(int)skin], SpawnPoint, Quaternion.identity);
      
      CreateAssociatedEcsEntity();

      return (int)skin;
    }

    private void CreateAssociatedEcsEntity()
    {
      EcsEntity shipEntity = WorldHandler.GetWorld().NewEntity();
      var convertToEntity = _playerShip.GetComponent<ConvertToEntity>();

      foreach (var component in _playerShip.GetComponents<Component>())
      {
        if (component is IConvertToEntity entityComponent)
        {
          entityComponent.Convert(shipEntity);
          GameObject.Destroy(component);
        }
      }
      
      ref var startPosition = ref shipEntity.Get<StartPosition>();
      startPosition.Position = _playerShip.transform.position;
      
      ref var shipState = ref shipEntity.Get<ShipState>();
      shipState.State = PlayerState.OnLevelStart;

      convertToEntity.Set(shipEntity);
      convertToEntity.setProccessed();
    }
  }
}