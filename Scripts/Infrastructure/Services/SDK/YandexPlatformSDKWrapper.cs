using System;
using DG.Tweening;
using StarGravity.Data;
using StarGravity.Infrastructure.Services.Localization;
using StarGravity.Infrastructure.Services.Progress;
using UnityEngine;

namespace StarGravity.Infrastructure.Services.SDK
{
  public class YandexPlatformSDKWrapper : ISDKWrapper
  {
    private readonly IProgressService _progress;
    private readonly ILocalizationService _localService;
    public bool SDKInited => YandexSDK.Instance.Inited;

    public event Action OnUserDataReceived;
    public event Action<string> OnGetLeaderboard;
    public event Action OnAdReward;
    public event Action OnAdRewardedOpened;
    public event Action OnAdRewardedClosed;
    public event Action OnAdRewardedError;
    public event Action<PurchasesCollection> OnGetPurchases;
    public event Action<string> OnPurchaseSuccess;
    public event Action<string> OnPurchaseFailed;
    public event Action ShowAuthInvite;

    public YandexPlatformSDKWrapper(IProgressService progressService, ILocalizationService localService)
    {
      _localService = localService;
      _progress = progressService;
    }

    public void Subscribe()
    {
      YandexSDK.Instance.OnUserDataReceived += (data) =>
      {
        _progress.UserData = JsonUtility.FromJson<UserData>(data);
        OnUserDataReceived?.Invoke();
      };
      YandexSDK.Instance.OnGetAppLanguage += SetLanguage;
      YandexSDK.Instance.onGetLeaderboard += (jsonData) => OnGetLeaderboard?.Invoke(jsonData);
      YandexSDK.Instance.onRewardedAdReward += (data) => OnAdReward?.Invoke();
      YandexSDK.Instance.onRewardedAdOpened += (data) => OnAdRewardedOpened?.Invoke();
      YandexSDK.Instance.onRewardedAdClosed += (data) => OnAdRewardedClosed?.Invoke();
      YandexSDK.Instance.onRewardedAdError += (data) => OnAdRewardedError?.Invoke();
      YandexSDK.Instance.onGetPurchases += (data) => OnGetPurchases?.Invoke(JsonUtility.FromJson<PurchasesCollection>(data));
      YandexSDK.Instance.onPurchaseSuccess += (purchaseId) => OnPurchaseSuccess?.Invoke(purchaseId);
      YandexSDK.Instance.onPurchaseFailed += (error) => OnPurchaseFailed?.Invoke(error);
      YandexSDK.Instance.showAuthentificateInvite += () =>
      {
        ShowAuthInvite?.Invoke();
      };
      YandexSDK.Instance.onGetUserLBData += ValidateUserLBData;
    }

    public void SaveScore(int score, int bonuses)
    {
      if (score <= _progress.UserData.LBBestScore && bonuses == 0)
        return;

      int scoreInc = 0;
      UserData user = _progress.UserData;
      
      if (score > _progress.UserData.LBBestScore)
      {
        SaveInLB(score, user);
        user.LBBestScore = score;
      }
      
      if (score > user.BestScore)
      {
        scoreInc = score - user.BestScore;
        user.BestScore = score;
      }
      if (bonuses > 0)
        user.Bonuses += bonuses;

      _progress.UserData = user;
      
      if (SDKInited)
        YandexSDK.Instance.SaveUserStats(scoreInc, bonuses);
      else
        _progress.Save();
    }

    private void SaveInLB(int score, UserData user)
    {
      if (SDKInited)
      {
        var payload = new LBPayload()
        {
          Skin = user.Skin,
          Hash = StringHash.GetHashForLbQuery(score, user.Skin, _progress.UserData.ID).ToString()
        };
        YandexSDK.Instance.SaveUserScoreToLb(score, JsonUtility.ToJson(payload));
      }
    }

    public bool BuySkin(string skin, int cost )
    {
      if (_progress.UserData.Bonuses >= cost)
      {
        UserData user = _progress.UserData;
        user.Bonuses -= cost;
        user.BoughtSkins.Add(skin);
        _progress.UserData = user;
        
        SaveProgress(0, -cost);

        return true;
      }

      return false;
    }

    public void ChangeSkin(string skin)
    { 
      UserData user = _progress.UserData;
      user.Skin = skin;
      _progress.UserData = user;
      if (SDKInited)
        YandexSDK.Instance.SaveAllDataToYandex(JsonUtility.ToJson(_progress.UserData));
      else
        _progress.Save();
    }

    public void GetLeaderboard()
    {
      if (!SDKInited)
        return;
      
      YandexSDK.Instance.Leaders();
    }

    public void ShowAd()
    {
      if (SDKInited)
        YandexSDK.Instance.ShowInterstitial();
    }

    public void ShowRewardedAd()
    {
      if (SDKInited)
        YandexSDK.Instance.ShowRewarded("121");
    }

    public bool ImproveBonus(int enhancementType, int cost)
    {
      if (_progress.UserData.Bonuses >= cost)
      {
        UserData user = _progress.UserData;
        user.Bonuses -= cost;
        user.Enhancements[enhancementType]++;
        _progress.UserData = user;
        
        SaveProgress(0, -cost);

        return true;
      }

      return false;
    }

    public void InitPurchases()
    {
      if (!SDKInited)
        return;
      
      YandexSDK.Instance.InitializePurchases();
    }

    public void Purchase(string id)
    {
      if (!SDKInited)
        return;
      
      YandexSDK.Instance.ProcessPurchase(id);
    }

    public void GetPurchases()
    {
      if (!SDKInited)
        return;
      
      YandexSDK.Instance.GetBoughtPurchases();
    }

    public void SaveProgress()
    {
      if (SDKInited)
        YandexSDK.Instance.SaveAllDataToYandex(JsonUtility.ToJson(_progress.UserData));
      else
        _progress.Save();
    }

    public void Authenticate()
    {
      YandexSDK.Instance.Authenticate();
    }

    private void SaveProgress(int scoreDif, int bonusesDif)
    {
      if (SDKInited)
      {
        YandexSDK.Instance.SaveUserStats(scoreDif, bonusesDif);
        YandexSDK.Instance.SaveAllDataToYandex(JsonUtility.ToJson(_progress.UserData));
      }
      else
        _progress.Save();
    }

    private void SetLanguage(string language)
    {
      _localService.ChangeLocale(language);
    }

    private void ValidateUserLBData(string data)
    {
      if (!_progress.IsValidBestScore(JsonUtility.FromJson<LBEntry>(data)))
      {
        var payload = new LBPayload()
        {
          Skin = _progress.UserData.Skin,
          Hash = StringHash.GetHashForLbQuery(1, _progress.UserData.Skin, _progress.UserData.ID).ToString()
        };
        YandexSDK.Instance.SaveUserScoreToLb(1, JsonUtility.ToJson(payload));
      }

      OnUserDataReceived?.Invoke();
    }
  }
}