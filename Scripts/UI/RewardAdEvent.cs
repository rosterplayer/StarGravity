using System;
using System.Collections.Generic;
using StarGravity.Infrastructure.Services.Ad;
using StarGravity.UI.Essentials.Popups;
using UnityEngine;
using VContainer;

namespace StarGravity.UI
{
  [RequireComponent(typeof(PopupOpener))]
  public class RewardAdEvent : MonoBehaviour
  {
    private AdService _adService;

    [Inject]
    public void Construct(AdService adService)
    {
      _adService = adService;
    }
    
    public void ShowRewardedAd()
    {
      _adService.ShowRewardedAd();
    }

    private void Start()
    {
      if (!_adService.IsReady)
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
      _adService.OnAdReward += OnGetAdReward;
    }

    private void OnDisable()
    {
      _adService.OnAdReward -= OnGetAdReward;
    }

    private void OnGetAdReward(int count)
    {
      GetComponent<PopupOpener>().OpenPopup(new List<object>() { count });
    }
  }
}