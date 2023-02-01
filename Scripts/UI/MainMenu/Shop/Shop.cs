using StarGravity.Infrastructure.Services.Progress;
using TMPro;
using UnityEngine;
using VContainer;

namespace StarGravity.UI.MainMenu.Shop
{
  public class Shop : MonoBehaviour
  {
    public TextMeshProUGUI BonusText;
    private IProgressService _progress;

    [Inject]
    public void Construct(IProgressService progress)
    {
      _progress = progress;
      SetBonusText();
      _progress.OnDataChanged += SetBonusText;
    }

    private void SetBonusText() => 
      BonusText.text = $"{_progress.UserData.Bonuses}";

    private void OnDestroy()
    {
      _progress.OnDataChanged -= SetBonusText;
    }
  }
}