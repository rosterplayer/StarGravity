using System;
using StarGravity.Data;
using UnityEngine;

namespace StarGravity.Infrastructure.Services.Progress
{
  public class ProgressService
  {
    private const string PlayerProgressKey = "PlayerProgress";
    private UserData _userData;
    public UserData UserData
    {
      get => _userData;
      set
      {
        _userData = value;
        OnDataChanged?.Invoke();
      }
    }

    public event Action OnDataChanged;

    public ProgressService()
    {
      UserData = new UserData();
    }

    public void Load()
    {
      string data = PlayerPrefs.GetString(PlayerProgressKey);
      
      if (string.IsNullOrEmpty(data))
        return;
      
      UserData = JsonUtility.FromJson<UserData>(data);
      //Debug.Log(data);
      //UserData.BoughtSkins = new List<string>();
      //UserData.Bonuses = -28;
      //UserData.Enhancements = new List<int> { 0, 0 };
    }

    public void Save()
    {
      PlayerPrefs.SetString(PlayerProgressKey, JsonUtility.ToJson(UserData));
    }
    
    public void ResetTutorials()
    {
      UserData.Tutorials = new int[5];
    }

    public bool IsValidBestScore(LBEntry userLBEntry)
    {
      LBPayload payload = JsonUtility.FromJson<LBPayload>(userLBEntry.extraData);
      string hash = StringHash.GetHashForLbQuery(userLBEntry.score, payload.Skin, UserData.ID).ToString();
      
      if (hash.Equals(payload.Hash))
      {
        UserData.LBBestScore = userLBEntry.score;
        return true;
      }
      
      UserData.LBBestScore = 1;
      return false;
    }
  }
}