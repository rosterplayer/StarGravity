using StarGravity.GamePlay.Planets;
using UnityEngine;

namespace StarGravity.GamePlay.Common.Randomizers
{
  [RequireComponent(typeof(SpriteRenderer))]
  public class RandomSprite : MonoBehaviour, IResetable
  {
    //Sprites that will be used randomly in this spriteRenderer
    public Sprite[] sprites;
    
    private SpriteRenderer _spriteRenderer;

    void Start () {
      _spriteRenderer = GetComponent<SpriteRenderer> ();
      Generate ();
    }

    public void Reset()
    {
      Generate();
    }

    //Generate and assign one of the sprites randomly

    private void Generate(){
      if(sprites.Length > 0){
        _spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
      }
    }
  }
}