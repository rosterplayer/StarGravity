using System.Collections.Generic;
using StarGravity.UI.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace StarGravity.UI.MainMenu.Shop
{
  public class ItemsContainer : MonoBehaviour
  {
    public ScrollRect Scroll;
    
    private readonly List<SkinItem> _items = new();

    public void SnapTo(RectTransform to)
    {
      Vector2 position = Scroll.GetSnapToPositionToBringChildIntoView(to);
      Scroll.content.localPosition = new Vector3(Mathf.Clamp(position.x, -Scroll.content.rect.width + Scroll.viewport.rect.width, 0), position.y);
    }

    public void RegisterItem(SkinItem item) => 
      _items.Add(item);

    public void ChangeItemsToNotActive()
    {
      foreach (SkinItem skinItem in _items)
      {
        skinItem.SetActiveItem(false);
      }
    }
  }
}