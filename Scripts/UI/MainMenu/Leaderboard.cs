using System;
using StarGravity.Data;
using StarGravity.Infrastructure.Services.Progress;
using StarGravity.Infrastructure.Services.SDK;
using UnityEngine;
using VContainer;

namespace StarGravity.UI.MainMenu
{
  public class Leaderboard : MonoBehaviour
  {
    public GameObject EntryPrefab;
    public Transform Container;
    private ISDKWrapper _sdk;
    private ProgressService _progressService;

    [Inject]
    public void Construct(ISDKWrapper sdkWrapper, ProgressService progressService)
    {
      _sdk = sdkWrapper;
      _progressService = progressService;
      Subscribe();
    }

    private void Subscribe()
    {
      _sdk.OnGetLeaderboard += FillLeaderboard;
      _sdk.GetLeaderboard();
    }

    private void FillLeaderboard(string leaderboardData)
    {
      LB data = JsonUtility.FromJson<LB>(leaderboardData);
      var playerId = _progressService.UserData.ID;
      foreach (LBEntry entry in data.entries)
      {
        LBPayload payload = JsonUtility.FromJson<LBPayload>(entry.extraData);
        string hash = StringHash.GetHashForLbQuery(entry.score, payload.Skin, entry.player.uniqueID).ToString();
        
        if (!hash.Equals(payload.Hash)) continue;
        
        var oneEntry = Instantiate(EntryPrefab, Container).GetComponent<LeaderboardEntry>();
        oneEntry.Set(
          entry.player.publicName,
          entry.rank,
          entry.score,
          Enum.TryParse(payload.Skin, out Skins skin) ? skin : Skins.Skin1,
          playerId == entry.player.uniqueID);
      }
    }

    private void OnDestroy()
    {
      _sdk.OnGetLeaderboard -= FillLeaderboard;
    }
  }
}