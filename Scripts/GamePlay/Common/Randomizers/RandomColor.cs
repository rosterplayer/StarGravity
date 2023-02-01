using StarGravity.GamePlay.Planets;
using UnityEngine;

namespace StarGravity.GamePlay.Common.Randomizers
{
  [RequireComponent(typeof(SpriteRenderer))]
  public class RandomColor : MonoBehaviour, IResetable
  {
    public Color[] colors;
    
    private SpriteRenderer _spriteRenderer;
    //From 0 to 100, if 0, always visible 
    [Range(0.0f, 100.0f)]
    public int invisibleProbability = 30;
    void Start () {
      _spriteRenderer = GetComponent<SpriteRenderer> ();
      Generate();
    }

    public void Reset()
    {
      Generate();
    }

    //Generate a new random color or hide the object
    private void Generate(){
      if (invisibleProbability > 0 && Random.Range (0, 100) < invisibleProbability) {
        _spriteRenderer.color = Color.clear;
        return;
      }
      int colorSelected = Random.Range (0, colors.Length);
      if (colors.Length > 0) {
        _spriteRenderer.color = colors[colorSelected];
      }
    }
  }
}