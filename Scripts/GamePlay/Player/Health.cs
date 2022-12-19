using System;
using StarGravity.Infrastructure.Services;
using StarGravity.Infrastructure.Services.Ad;
using StarGravity.Infrastructure.Services.Progress;
using StarGravity.UI.MainMenu.Shop.Enhancements;
using UnityEngine;
using VContainer;

namespace StarGravity.GamePlay.Player
{
  public class Health : MonoBehaviour
  {
    private const float ConstantHealthTime = 10;
    
    public int HealthPoints;

    private float _constantHealthTimeRemains;
    private AdService _adService;
    private ProgressService _progressService;

    public event Action<int> HealthChanged;
    public event Action OnDied;
    public event Action<float> OnConstantHealth;

    [Inject]
    public void Construct(AdService adService, ProgressService progressService)
    {
      _progressService = progressService;
      _adService = adService;
    }

    private void Start()
    {
      if (_adService.IsRewardedBonusActive())
        HealthPoints++;
      
      HealthChanged?.Invoke(HealthPoints);
    }

    private void Update()
    {
      if (_constantHealthTimeRemains > 0)
        _constantHealthTimeRemains -= Time.deltaTime;
    }

    public void SetConstantHealth()
    {
      _constantHealthTimeRemains += 
        ConstantHealthTime + 0.1f * ConstantHealthTime * _progressService.UserData.Enhancements[(int)EnhancementType.HealthBonus];
      OnConstantHealth?.Invoke(_constantHealthTimeRemains);
    }

    public void DecreaseHealth()
    {
      if (_constantHealthTimeRemains > 0)
        return;
      
      HealthPoints--;
      HealthChanged?.Invoke(HealthPoints);

      if (HealthPoints <= 0)
      {
        OnDied?.Invoke();
        Destroy(gameObject);
      }
    }
    
    public void IncreaseHealth()
    {
      HealthPoints++;
      HealthChanged?.Invoke(HealthPoints);
    }
  }
}