using StarGravity.Infrastructure.Services.Input;
using StarGravity.UI.Essentials.Popups;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace StarGravity.UI.Tutorials
{
  public class Tutorial : MonoBehaviour
  {
    public GameObject[] Slides;
    public Button NextSlideButton;
    public Button CloseButton;
    public Popup Popup;

    private int _activeSlide;
    
    private IInputService _inputService;

    [Inject]
    public void Construct(IInputService inputService)
    {
      _inputService = inputService;
    }

    private void Start()
    {
      _inputService.ReleaseControl();
      SwitchActiveSlide();
      if (IsLastSlideActive())
        SwitchButtons();
    }

    public void ChangeSlide()
    {
      if (IsLastSlideActive())
        return;

      _activeSlide++;
      SwitchActiveSlide();
      if (IsLastSlideActive())
        SwitchButtons();
    }

    public void Close()
    {
      Popup.Close();
      _inputService.GainControl();
    }

    private bool IsLastSlideActive() => 
      _activeSlide == Slides.Length - 1;

    private void SwitchButtons()
    {
      NextSlideButton.gameObject.SetActive(false);
      CloseButton.gameObject.SetActive(true);
    }

    private void SwitchActiveSlide()
    {
      for (int i = 0; i < Slides.Length; i++)
      {
        Slides[i].SetActive(i == _activeSlide);
      }
    }
  }
}