using System.Collections.Generic;
using StarGravity.Infrastructure.Factories;
using UnityEngine;
using VContainer;

namespace StarGravity.UI.Essentials.Popups
{
  public class PopupOpener : MonoBehaviour
  {
    public GameObject popupPrefab;
    [SerializeField] private Canvas _canvas;
    
    private IPopupFactory _factory;

    [Inject]
    public void Construct(IPopupFactory factory)
    {
      _factory = factory;
    }

    public virtual void OpenPopup()
    {
      _factory.Create(popupPrefab, _canvas.transform);
    }
    
    public virtual void OpenPopup(List<object> args)
    {
      GameObject popup = _factory.Create(popupPrefab, _canvas.transform);
      popup.GetComponent<DynamicLocalizationText>().ChangeText(args);
    }
  }
}