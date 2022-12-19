using UnityEngine;

namespace StarGravity.Data
{
  [CreateAssetMenu(fileName = "GameParameters", menuName = "Assets/GameParameters", order = 0)]
  public class GameParameters : ScriptableObject
  {
    // don't change field names or the reflexion will break
    public int Skin1Cost;
    public int Skin2Cost;
    public int Skin3Cost;
    public int Skin4Cost;
    public int Skin5Cost;

    public int[] EnhancementsCost;
  }
}