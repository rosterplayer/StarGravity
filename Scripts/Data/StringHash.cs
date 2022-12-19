using UnityEngine;

namespace StarGravity.Data
{
  public static class StringHash
  {
    private const string PrivateKey = "weiri3o4u54";
    public static Hash128 GetHash(string input)
    {
      Hash128 hash = new Hash128();
      hash.Append(input);
      return hash;
    }

    public static Hash128 GetHashForLbQuery(int score, string skin, string id) => 
      GetHash($"{score}_{skin}_{id}_{PrivateKey}");
  }
}