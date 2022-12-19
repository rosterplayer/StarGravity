using System;
using StarGravity.Data;
using StarGravity.Infrastructure.Services.Progress;
using StarGravity.Infrastructure.Services.SDK;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using ToggleGroup = StarGravity.UI.HUDElements.ToggleGroup;

namespace StarGravity.UI.MainMenu.Shop.Enhancements
{
  public enum EnhancementType
  {
    HealthBonus = 0,
    MagnetBonus = 1
  }

  public class ImproveBar : MonoBehaviour
  {
    [SerializeField] private EnhancementType _enhancementType;
    [SerializeField] private ToggleGroup _progressBar;
    [SerializeField] private TextMeshProUGUI _cost;
    [SerializeField] private Button _improveButton;
    
    private ISDKWrapper _sdk;
    private ProgressService _progress;
    private GameParameters _gameParameters;

    [Inject]
    public void Construct(ISDKWrapper sdkWrapper, ProgressService progressService, GameParameters gameParameters)
    {
      _gameParameters = gameParameters;
      _progress = progressService;
      _sdk = sdkWrapper;
    }

    private void Start()
    {
      UpdateVisual();
      _improveButton.onClick.AddListener(ImproveBonus);
    }

    private void ImproveBonus()
    {
      int enhancementLevel = _progress.UserData.Enhancements[(int)_enhancementType];
      int enhancementMaxLevel = _gameParameters.EnhancementsCost.Length;
      
      if (enhancementLevel >= enhancementMaxLevel)
        return;
      
      if (_sdk.ImproveBonus((int)_enhancementType, _gameParameters.EnhancementsCost[enhancementLevel])) 
        UpdateVisual();
    }

    private void UpdateVisual()
    {
      int enhancement = _progress.UserData.Enhancements[(int)_enhancementType];
      _progressBar.Switch(1 + enhancement);
      _cost.text = $"{_gameParameters.EnhancementsCost[Math.Clamp(enhancement, 0, _gameParameters.EnhancementsCost.Length - 1)]}";
    }
  }
}