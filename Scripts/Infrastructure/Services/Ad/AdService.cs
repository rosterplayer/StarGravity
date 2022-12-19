using System;
using StarGravity.Infrastructure.Services.SDK;
using StarGravity.Infrastructure.Services.Sound;

namespace StarGravity.Infrastructure.Services.Ad
{
  public class AdService
  {
    private const int Attempts = 10;
    private const int RestartsToShowAd = 3;
    private const int AdCooldown = 30;

    private int _levelRestarted;
    private int _rewardedAdBonusCounter;
    private DateTime _lastAdCallTime = DateTime.Now;

    private readonly ISDKWrapper _sdk;
    private readonly SoundService _soundService;
    public bool IsReady => _sdk.SDKInited;

    public event Action<int> OnAdReward;

    public AdService(ISDKWrapper sdk, SoundService soundService)
    {
      _soundService = soundService;
      _sdk = sdk;
      _sdk.OnAdReward += () =>
      {
        SwitchOnSound();
        SetBonusOnRewardedAd();
      };
      _sdk.OnAdRewardedOpened += SwitchOffSound;
      _sdk.OnAdRewardedClosed += SwitchOnSound;
      _sdk.OnAdRewardedError += SwitchOnSound;
    }

    private void SwitchOnSound()
    {
      _soundService.UnpauseSound();
    }

    private void SwitchOffSound()
    {
      _soundService.PauseSound();
    }

    public bool IsRewardedBonusActive() => 
      _rewardedAdBonusCounter >= 0;

    public void DecreaseRewardedAdBonusCounter() => 
      _rewardedAdBonusCounter--;

    public void ShowAd()
    {
      _levelRestarted++;
      
      if (IsADReady())
      {
        _lastAdCallTime = DateTime.Now;
        _levelRestarted = 0;
        _sdk.ShowAd();
      }
    }

    public void ShowRewardedAd()
    {
      if (_rewardedAdBonusCounter > 0)
      {
        OnAdReward?.Invoke(_rewardedAdBonusCounter);
        return;
      }
      //SetBonusOnRewardedAd();
      _sdk.ShowRewardedAd();
    }

    private void SetBonusOnRewardedAd()
    {
      _rewardedAdBonusCounter = Attempts;
      OnAdReward?.Invoke(_rewardedAdBonusCounter);
    }

    private bool IsADReady()
    {
      TimeSpan time = DateTime.Now - _lastAdCallTime;
      return _levelRestarted >= RestartsToShowAd && time.TotalSeconds > AdCooldown;
    }
  }
}