using UnityEngine;

namespace StarGravity.GamePlay.Planets
{
  public class ScaleCorrector : MonoBehaviour
  {
    public RandomSize[] Sizes;
    private Vector3 _initialScale;

    private void Start()
    {
      _initialScale = transform.localScale;
      Correct();
    }

    private void Correct()
    {
      float scaleMultiplier = 1;
      foreach (RandomSize size in Sizes)
      {
        scaleMultiplier *= size.ScaleMultiplier;
      }

      transform.localScale = _initialScale * scaleMultiplier;
    }
  }
}