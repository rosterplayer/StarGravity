using System;
using Leopotam.Ecs;
using StarGravity.GamePlay.Common.Components;
using StarGravity.GamePlay.Utilities;
using StarGravity.Infrastructure.Services;
using StarGravity.Infrastructure.Services.Ad;
using StarGravity.Infrastructure.Services.Progress;
using StarGravity.UI.MainMenu.Shop.Enhancements;
using UnityEngine;
using VContainer;
using Voody.UniLeo;

namespace StarGravity.GamePlay.Player
{
  public class Health : MonoBehaviour
  {
    private const float ConstantHealthTime = 10;
    
    public int HealthPoints;

    private float _constantHealthTimeRemains;
    private IAdService _adService;
    private IProgressService _progressService;

    public event Action<int> HealthChanged;
    public event Action OnDied;
    public event Action<float> OnConstantHealth;

    [Inject]
    public void Construct(IAdService adService, IProgressService progressService)
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
        if (gameObject.GetComponent<ConvertToEntity>().TryGetEcsEntity(out EcsEntity entity))
        {
          entity.Get<ForDestroy>();
        }
      }
    }
    
    public void IncreaseHealth()
    {
      HealthPoints++;
      HealthChanged?.Invoke(HealthPoints);
    }
  }
}