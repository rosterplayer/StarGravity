using StarGravity.Infrastructure.Services.Progress;
using UnityEngine;
using VContainer;

namespace StarGravity.UI.MainMenu
{
  public class ResetTutorials : MonoBehaviour
  {
    private ProgressService _progressService;

    [Inject]
    public void Construct(ProgressService progressService)
    {
      _progressService = progressService;
    }

    public void ResetAll()
    {
      _progressService.ResetTutorials();
    }
  }
}