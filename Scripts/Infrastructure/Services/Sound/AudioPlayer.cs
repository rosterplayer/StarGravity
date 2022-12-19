using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace StarGravity.Infrastructure.Services.Sound
{
  public enum SfxType
  {
    Bonus = 0,
    ShipCrash = 1,
    HPBonus = 2,
    Lose = 3,
    Shield = 4,
    CaptureField = 5,
    Invisibility = 6,
    ManoeuvreEngine = 7,
    AsteroidCrash = 8,
    HeavyBonus = 9
  }

  public enum SceneType
  {
    Menu = 0,
    Game = 1
  }

  public class AudioPlayer : MonoBehaviour
  {
    [Header("Audio sources")]
    public AudioSource SfxAudioSource;
    public AudioSource MusicAudioSource;

    [Header("SFX")] 
    public AudioClip CollectBonus;
    public AudioClip Crash;
    public AudioClip CollectHPBonus;
    public AudioClip Lose;
    public AudioClip ShieldEffect;
    public AudioClip CaptureEffect;
    public AudioClip InvisibilityEffect;
    public AudioClip ManoeuvreEngineEffect;
    public AudioClip AsteroidCrashEffect;
    public AudioClip HeavyBonusEffect;

    [Header("Music")]
    public AudioClip[] MenuMusic;
    public AudioClip[] GameMusic;
    
    private Coroutine _musicLoop;

    private void Awake()
    {
      DontDestroyOnLoad(this);
    }

    public void PlayMusic(SceneType type)
    {
      StopMusic();
      AudioClip clip = GetRandomMusic(type);
      MusicAudioSource.clip = clip;
      MusicAudioSource.Play();
      _musicLoop = StartCoroutine(WaitToPlayNextMusic(type, clip.length));
    }

    public void StopMusic()
    {
      if (_musicLoop != null)
        StopCoroutine(_musicLoop);
      MusicAudioSource.Stop();
    }

    public void PlaySFX(SfxType sound) => 
      SfxAudioSource.PlayOneShot(GetAudioClipByName(sound));

    private IEnumerator WaitToPlayNextMusic(SceneType type, float delay)
    {
      yield return new WaitForSeconds(delay);
      PlayMusic(type);
    }

    private AudioClip GetRandomMusic(SceneType type) => 
      type == SceneType.Menu ? MenuMusic[Random.Range(0, MenuMusic.Length)] : GameMusic[Random.Range(0, GameMusic.Length)];

    private AudioClip GetAudioClipByName(SfxType soundName)
    {
      switch (soundName)
      {
        case SfxType.Bonus:
          return CollectBonus;
        case SfxType.ShipCrash:
          return Crash;
        case SfxType.HPBonus:
          return CollectHPBonus;
        case SfxType.Lose:
          return Lose;
        case SfxType.Shield:
          return ShieldEffect;
        case SfxType.CaptureField:
          return CaptureEffect;
        case SfxType.Invisibility:
          return InvisibilityEffect;
        case SfxType.ManoeuvreEngine:
          return ManoeuvreEngineEffect;
        case SfxType.AsteroidCrash:
          return AsteroidCrashEffect;
        case SfxType.HeavyBonus:
          return HeavyBonusEffect;
        default:
          Debug.Log("unknown sound name");
          return null;
      }
    }
  }
}