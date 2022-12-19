using System;
using StarGravity.Data;

namespace StarGravity.Infrastructure.Services.SDK
{
  public interface ISDKWrapper
  {
    bool SDKInited { get; }
    event Action OnUserDataReceived;
    event Action<string> OnGetLeaderboard;
    event Action OnAdReward;
    event Action<PurchasesCollection> OnGetPurchases;
    event Action<string> OnPurchaseSuccess;
    event Action<string> OnPurchaseFailed;
    event Action ShowAuthInvite;
    void SaveScore(int score, int bonuses);
    bool BuySkin(string skin, int cost );
    void ChangeSkin(string skin);
    void GetLeaderboard();
    void Subscribe();
    void ShowAd();
    void ShowRewardedAd();
    bool ImproveBonus(int enhancementType, int cost);
    void InitPurchases();
    void Purchase(string id);
    void GetPurchases();
    void SaveProgress();
    void Authenticate();
    event Action OnAdRewardedOpened;
    event Action OnAdRewardedClosed;
    event Action OnAdRewardedError;
  }
}