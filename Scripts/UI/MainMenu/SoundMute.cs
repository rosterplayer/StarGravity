using StarGravity.Infrastructure.Services.Sound;
using UnityEngine;
using VContainer;

namespace StarGravity.UI.MainMenu
{
  public class SoundMute : MonoBehaviour
  {
    private const string Sound = "sound";
    public GameObject Muted;
    public GameObject Unmuted;
    
    private bool _muted;
    private SoundService _soundService;

    [Inject]
    public void Construct(SoundService soundService)
    {
      _soundService = soundService;
      
      if (GetSoundToggle() == 0)
        ToggleSound();
    }

    public void ToggleSound()
    {
      _muted = !_muted;
      SaveSoundToggle();
      if (_muted)
      {
        _soundService.SwitchSoundOff();
        Muted.SetActive(false);
        Unmuted.SetActive(true);
      }
      else
      {
        _soundService.SwitchSoundOn();
        Muted.SetActive(true);
        Unmuted.SetActive(false);
      }
    }

    private void SaveSoundToggle() => 
      PlayerPrefs.SetInt(Sound, _muted ? 0 : 1);
    
    private int GetSoundToggle() => 
      PlayerPrefs.HasKey(Sound) ? PlayerPrefs.GetInt(Sound) : 1;
  }
}