using UnityEngine;

namespace StarGravity.Infrastructure.Services.Sound
{
  public class SoundService
  {
    private readonly AudioPlayer _audioPlayer;
    private bool _soundPaused;

    public SoundService(AudioPlayer audio)
    {
      _audioPlayer = audio;
    }

    public void SwitchSoundOff()
    {
      AudioListener.volume = 0;
    }
    
    public void SwitchSoundOn()
    {
      AudioListener.volume = 1;
    }

    public void PauseSound()
    {
      if (AudioListener.volume == 0)
        return;

      _soundPaused = true;
      SwitchSoundOff();
    }

    public void UnpauseSound()
    {
      if (AudioListener.volume > 0 || !_soundPaused)
        return;

      _soundPaused = false;
      SwitchSoundOn();
    }

    public void PlayMusic(SceneType type) => 
      _audioPlayer.PlayMusic(type);

    public void StopMusic() =>
      _audioPlayer.StopMusic();

    public void PlaySFX(SfxType sound) =>
      _audioPlayer.PlaySFX(sound);
  }
}