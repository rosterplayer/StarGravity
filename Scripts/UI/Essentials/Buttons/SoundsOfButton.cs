using UnityEngine;

namespace StarGravity.UI.Essentials.Buttons
{
  [RequireComponent(typeof(AudioSource))]
  public class SoundsOfButton : MonoBehaviour
  {
    public AudioClip PressedSound;
    public AudioClip RolloverSound;

    private AudioSource _audioSource;

    private void Awake()
    {
      _audioSource = GetComponent<AudioSource>();
    }

    public void PlayPressedSound()
    {
      _audioSource.clip = PressedSound;
      _audioSource.Play();
    }

    public void PlayRolloverSound()
    {
      _audioSource.clip = RolloverSound;
      _audioSource.Play();
    }
  }
}