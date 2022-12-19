using StarGravity.GamePlay.Player;
using StarGravity.GamePlay.Player.Perks;
using StarGravity.Infrastructure.Factories;
using StarGravity.Infrastructure.Services.Ad;
using StarGravity.Infrastructure.Services.Progress;
using UnityEngine;
using VContainer;

namespace StarGravity.UI.HUDElements
{
  public class HUD : MonoBehaviour
  {
    public PointsPanel VisitedSystems;
    public PointsPanel TotalPoints;
    public ToggleGroup Health3;
    public ToggleGroup Health4;
    public CooldownPanel ShieldPanel;
    public CooldownPanel CapturePanel;
    public CooldownPanel InvisibilityPanel;
    public CooldownPanel ManoeuvrePanel;
    public BonusLatencyBar HpBonusBar;
    public BonusLatencyBar MagnetBonusBar;

    private StarShip _playerShip;
    private Health _health;
    private PopupFactory _popupFactory;
    private GameLevelProgressService _levelProgress;
    private AdService _adService;

    [Inject]
    public void Construct(PlayerShipFactory playerShipFactory, PopupFactory popupFactory, GameLevelProgressService levelProgress, AdService adService)
    {
      _adService = adService;
      _levelProgress = levelProgress;
      _playerShip = playerShipFactory.PlayerShip.GetComponent<StarShip>();
      _health = playerShipFactory.PlayerShip.GetComponent<Health>();
      _popupFactory = popupFactory;

      Subscribe();
      InitPerkPanel();
      InitHealthPanel();
    }

    private void Subscribe()
    {
      _levelProgress.ProgressChanged += ChangePointsAndSystemsText;
      _playerShip.OnMagnetActivated += MagnetBonusBar.OnStart;
      _health.HealthChanged += ChangeHealth;
      _health.OnConstantHealth += HpBonusBar.OnStart;
      _health.OnDied += OpenGameOverPopup;
    }

    private void InitPerkPanel()
    {
      ShieldPanel.gameObject.SetActive(false);
      CapturePanel.gameObject.SetActive(false);
      InvisibilityPanel.gameObject.SetActive(false);
      ManoeuvrePanel.gameObject.SetActive(false);

      if (_playerShip.ShipPerk == null)
        return;

      switch (_playerShip.ShipPerk)
      {
        case Shield perk:
          ShieldPanel.gameObject.SetActive(true);
          ShieldPanel.Initialize(perk);
          break;
        case CaptureField capture:
          CapturePanel.gameObject.SetActive(true);
          CapturePanel.Initialize(capture);
          break;
        case InvisibilityField invisibility:
          InvisibilityPanel.gameObject.SetActive(true);
          InvisibilityPanel.Initialize(invisibility);
          break;
        case Manoeuvre manoeuvre:
          ManoeuvrePanel.gameObject.SetActive(true);
          ManoeuvrePanel.Initialize(manoeuvre);
          break;
      }
    }

    private void InitHealthPanel()
    {
      if (_adService.IsRewardedBonusActive())
      {
        Health3.gameObject.SetActive(false);
        Health4.gameObject.SetActive(true);
      }
      else
      {
        Health3.gameObject.SetActive(true);
        Health4.gameObject.SetActive(false);
      }
    }

    private void OpenGameOverPopup() =>
      _popupFactory.CreateGameOverPopup(GetComponent<Canvas>().transform);

    private void ChangeHealth(int hp)
    {
      if (_adService.IsRewardedBonusActive())
        Health4.Switch(hp);
      else
        Health3.Switch(hp);
    }

    private void ChangePointsAndSystemsText()
    {
      TotalPoints.SetText(_levelProgress.LevelProgress.TotalPoints);
      VisitedSystems.SetText(_levelProgress.LevelProgress.Systems);
    }

    private void OnDisable()
    {
      Unsubscribe();
    }

    private void Unsubscribe()
    {
      _levelProgress.ProgressChanged -= ChangePointsAndSystemsText;
      _playerShip.OnMagnetActivated -= MagnetBonusBar.OnStart;
      _health.HealthChanged -= ChangeHealth;
      _health.OnConstantHealth -= HpBonusBar.OnStart;
      _health.OnDied -= OpenGameOverPopup;
    }
  }
}