using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StarGravity.UI.Essentials.Buttons
{
  public class ButtonWithSound : Button
  {
    private SoundsOfButton _soundsOfButton;
    private bool _pointerWasUp;
    
    protected override void Awake()
    {
      base.Awake();
      _soundsOfButton = GetComponent<SoundsOfButton>();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
      if (_soundsOfButton != null && interactable)
      {
        _soundsOfButton.PlayPressedSound();
      }
      base.OnPointerClick(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
      _pointerWasUp = true;
      base.OnPointerUp(eventData);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
      if (_pointerWasUp)
      {
        _pointerWasUp = false;
        base.OnPointerEnter(eventData);
      }
      else
      {
        if (_soundsOfButton != null && interactable)
        {
          _soundsOfButton.PlayRolloverSound();
        }
        base.OnPointerEnter(eventData);
      }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
      _pointerWasUp = false;
      base.OnPointerExit(eventData);
    }
  }
}