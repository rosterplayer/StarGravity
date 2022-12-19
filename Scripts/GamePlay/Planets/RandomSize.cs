using UnityEngine;

namespace StarGravity.GamePlay.Planets
{
  public class RandomSize : MonoBehaviour
  {
    [Range(1.0f, 10.0f)]
    public float multiplierMax = 3f;

    private Vector3 _initialScale;
    private float _randomScaleMultiplier;

    public float ScaleMultiplier => _randomScaleMultiplier;

    void Awake () {
      //Initial scale
      _initialScale = transform.localScale;
      Generate();
    }

    private void Generate()
    {
      //Choose a random multiplied scale from the initial scale and the multiplierMax variable
      _randomScaleMultiplier = Random.Range(1f, multiplierMax);
      transform.localScale = _initialScale * _randomScaleMultiplier;
    }
  }
}