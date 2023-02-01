using StarGravity.Infrastructure.Services.Progress;
using UnityEngine;
using VContainer;

namespace StarGravity.UI.MainMenu
{
  public class ResetTutorials : MonoBehaviour
  {
    private IProgressService _progressService;

    [Inject]
    public void Construct(IProgressService progressService)
    {
      _progressService = progressService;
    }

    public void ResetAll()
    {
      _progressService.ResetTutorials();
    }
  }
}