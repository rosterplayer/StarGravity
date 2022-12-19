using TMPro;
using UnityEngine;

namespace StarGravity.UI.HUDElements
{
  public class PointsPanel : MonoBehaviour
  {
    public TextMeshProUGUI Points;
    public AddPointsAnimation PointsAnimation;

    public void SetText(int newPoints)
    {
      int inc = newPoints - int.Parse(Points.text);
      if (inc <= 0) 
        return;
      
      PointsAnimation.SetPoints(inc);
      PointsAnimation.Play();
      Points.text = $"{newPoints}";
    }
  }
}