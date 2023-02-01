using StarGravity.Infrastructure.AssetManagement;
using StarGravity.UI;
using StarGravity.UI.Essentials;
using StarGravity.UI.Essentials.Popups;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace StarGravity.Infrastructure.Factories
{
  public class PopupFactory : IPopupFactory
  {
    private readonly IObjectResolver _container;
    private readonly GamePrefabs _gamePrefabs;
    private readonly Canvas _parentCanvas;

    public PopupFactory(IObjectResolver container, GamePrefabs prefabs, Canvas parentCanvas)
    {
      _container = container;
      _gamePrefabs = prefabs;
      _parentCanvas = parentCanvas;
    }

    public GameObject Create(GameObject prefab, Transform parent = null)
    {
      GameObject popup = _container.Instantiate(prefab, parent != null ? parent : _parentCanvas.transform);
      popup.SetActive(true);
      popup.transform.localScale = Vector3.zero;
      popup.GetComponent<Popup>().Open();
      return popup;
    }

    public GameObject CreateGameOverPopup(Transform parent = null) => 
      Create(_gamePrefabs.GameOverPopup, parent);
  }
}