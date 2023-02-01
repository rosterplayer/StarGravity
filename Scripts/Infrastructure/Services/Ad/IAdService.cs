using System;

namespace StarGravity.Infrastructure.Services.Ad
{
  public interface IAdService
  {
    bool IsReady { get; }
    event Action<int> OnAdReward;
    bool IsRewardedBonusActive();
    void DecreaseRewardedAdBonusCounter();
    void ShowAd();
    void ShowRewardedAd();
  }
}