using UnityEngine;
using UnityEngine.UI;

namespace StarGravity.UI.MainMenu.Shop
{
  public class BackgroundColor : MonoBehaviour
  {
    public Color ActiveBgColor;
    public Color ActiveBorderColor;
    public Color NotActiveBgColor;
    public Color NotActiveBorderColor;
    public Image Background;
    public Image Border;

    public void SetActiveColor(bool isActive)
    {
      if (isActive)
      {
        Background.color = ActiveBgColor;
        Border.color = ActiveBorderColor;
      }
      else
      {
        Background.color = NotActiveBgColor;
        Border.color = NotActiveBorderColor;
      }
    }
  }
}