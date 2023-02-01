using StarGravity.Data;
using StarGravity.UI.Essentials;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StarGravity.UI.MainMenu.Leaderboard
{
  public class LeaderboardEntry : MonoBehaviour
  {
    public TextMeshProUGUI Rank;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Points;
    public ImageChooser Skin;
    public Image ItsMeBorder;
    
    public void Set(string playerName, int rank, int points, Skins skin, bool itsMe)
    {
      Rank.text = $"{rank}";
      Name.text = $"{playerName}";
      Points.text = $"{points}";
      Skin.SetImage(skin);
      if (itsMe)
        ItsMeBorder.gameObject.SetActive(true);
    }
  }
}