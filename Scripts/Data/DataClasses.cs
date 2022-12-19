using System;
using System.Collections.Generic;

namespace StarGravity.Data
{
  public class LB {
    public List<LBEntry> entries;
    public LBInfo leaderboard;
    public List<Ranges> ranges;
    public int userRank;
  }

  [Serializable]
  public class LBInfo {
    public int appID;
    public string name;
  }

  [Serializable]
  public class LBEntry {
    public int score;
    public string extraData;
    public int rank;
    public Player player;
    public string formattedScore;
  }
  
  [Serializable]
  public class Player {
    public string getAvatarSrc;
    public string getAvatarSrcSet;
    public string lang;
    public string publicName;
    public string uniqueID;
  }
  
  [Serializable]
  public class Ranges {
    public int start;
    public int size;
  }

  [Serializable]
  public class Purchase
  {
    public string productID;
    public long purchaseTime;
    public string purchaseToken;
    public string developerPayload;
  }

  [Serializable]
  public class LBPayload
  {
    public string Skin;
    public string Hash;
  }

  [Serializable]
  public class PurchasesCollection
  {
    public Purchase[] purchases;
  }
  
  [Serializable]
  public class UserData {
    public string ID;
    public string Name;
    public int BestScore;
    public int LBBestScore;
    public int Bonuses;
    public string Skin;
    public List<string> BoughtSkins;
    public List<int> Enhancements;
    public int[] Tutorials;

    public UserData()
    {
      ID = "";
      Name = "";
      BestScore = 0;
      LBBestScore = 0;
      Bonuses = 0;
      Skin = "Skin1";
      BoughtSkins = new List<string>();
      // Enhancements[0] - hp bonus latency level, Enhancements[1] - magnet latency
      Enhancements = new List<int> {0, 0};
      Tutorials = new int[5];
    }
    
    public int FindSkin(string skin) => 
      BoughtSkins.FindIndex(x => x == skin);
  }
}