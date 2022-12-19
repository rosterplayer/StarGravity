using StarGravity.Data;
using UnityEngine;
using UnityEngine.UI;

namespace StarGravity.UI.Essentials
{
  public class ImageChooser : MonoBehaviour
  {
    public Sprite[] Pictures;
    public Image Image;

    public void SetImage(Skins skin)
    {
      Image.sprite = Pictures[(int)skin];
    }
  }
}