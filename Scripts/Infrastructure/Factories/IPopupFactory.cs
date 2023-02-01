using UnityEngine;

namespace StarGravity.Infrastructure.Factories
{
  public interface IPopupFactory
  {
    GameObject Create(GameObject prefab, Transform parent = null);
    GameObject CreateGameOverPopup(Transform parent = null);
  }
}