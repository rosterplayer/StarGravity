using UnityEngine;

namespace StarGravity.GamePlay.Planets
{
  [RequireComponent(typeof(SpriteRenderer))]
  public class RandomSprite : MonoBehaviour
  {
    //Sprites that will be used randomly in this spriteRenderer
    public Sprite[] sprites;
    
    private SpriteRenderer _spriteRenderer;

    void Start () {
      _spriteRenderer = GetComponent<SpriteRenderer> ();
      Generate ();
    }

    //Generate and assign one of the sprites randomly
    private void Generate(){
      if(sprites.Length > 0){
        _spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
      }
    }
  }
}