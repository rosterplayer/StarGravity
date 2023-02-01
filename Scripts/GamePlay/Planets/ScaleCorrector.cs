using StarGravity.GamePlay.Common.Randomizers;
using UnityEngine;

namespace StarGravity.GamePlay.Planets
{
  public class ScaleCorrector : MonoBehaviour, IResetable
  {
    public RandomSize[] Sizes;
    private Vector3 _initialScale;

    private void Start()
    {
      _initialScale = transform.localScale;
      Correct();
    }

    public void Reset()
    {
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