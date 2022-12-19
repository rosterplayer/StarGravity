using StarGravity.Infrastructure.Services.Input;
using StarGravity.UI.Essentials.Popups;
using UnityEngine.SceneManagement;
using VContainer;

namespace StarGravity.UI.HUDElements
{
  public class PausePopup : Popup
  {
    private IInputService _inputService;

    [Inject]
    public void Construct(IInputService inputService)
    {
      _inputService = inputService;
    }
    public void Exit()
    {
      SceneManager.LoadScene("Menu");
    }

    public void Restart()
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Start()
    {
      _inputService.ReleaseControl();
    }

    private void OnDisable()
    {
      _inputService.GainControl();
    }
  }
}