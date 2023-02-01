using StarGravity.Data;
using StarGravity.Infrastructure.Services.InApp;
using StarGravity.Infrastructure.Services.Progress;
using StarGravity.Infrastructure.Services.SDK;
using TMPro;
using UnityEngine;
using VContainer;

namespace StarGravity.UI.MainMenu.Shop
{
  public class SkinItem : MonoBehaviour
  {
    private const string OpenAllShipsKey = "open_all_ships";
    
    public Skins Skin;
    public GameObject BuyButton;
    public TextMeshProUGUI BuyBtnCostText;
    public GameObject LockedButton;
    public TextMeshProUGUI LockedBtnCostText;
    public GameObject BoughtButton;
    public BackgroundColor BackgroundColor;

    private int _costValue;
    private bool _activeSkin;
    private bool _allShipsBought;
    private ItemsContainer _container;

    private ISDKWrapper _sdk;
    private IProgressService _progress;
    private GameParameters _gameParams;
    private IInAppService _inAppService;

    [Inject]
    public void Construct(ISDKWrapper sdkWrapper, IProgressService progressService, GameParameters gameParameters, IInAppService inAppService)
    {
      _sdk = sdkWrapper;
      _progress = progressService;
      _gameParams = gameParameters;
      _inAppService = inAppService;
      _costValue = (int)_gameParams.GetType().GetField($"{Skin.ToString()}Cost").GetValue(_gameParams);

      Subscribe();
    }

    private void Awake()
    {
      _container = gameObject.GetComponentInParent<ItemsContainer>();
      _container.RegisterItem(this);
    }

    private void Start()
    {
      BuyBtnCostText.text = $"{_costValue}";
      LockedBtnCostText.text = $"{_costValue}";
      CheckAllShipsOpened();

      if (IsBoughtSkin())
        SwitchBetweenBtns(BoughtButton);
      else
        CheckForEnoughBonuses();

      if (HaveToActive())
      {
        SetActiveItem(true);
        _container.SnapTo(GetComponent<RectTransform>());
      }
    }

    private void OnDestroy()
    {
      Unsubscribe();
    }

    private void Subscribe()
    {
      _inAppService.OnPurchaseSuccess += OnPurchaseOpenAllShips;
      _inAppService.PurchasesUpdated += CheckAllShipsOpened;
    }

    private void Unsubscribe()
    {
      _inAppService.OnPurchaseSuccess -= OnPurchaseOpenAllShips;
      _inAppService.PurchasesUpdated -= CheckAllShipsOpened;
    }

    private void OnPurchaseOpenAllShips(string purchaseId)
    {
      if (purchaseId == OpenAllShipsKey)
      {
        _allShipsBought = true;
        SwitchBetweenBtns(BoughtButton);
      }
    }

    private void CheckAllShipsOpened()
    {
      if (_inAppService.FindInPurchasedItems(OpenAllShipsKey) != null)
      {
        _allShipsBought = true;
        SwitchBetweenBtns(BoughtButton);
      }
    }

    public void SetActiveItem(bool isActive)
    {
      _activeSkin = isActive;
      BackgroundColor.SetActiveColor(isActive);
    }

    public void BuySkin()
    {
      if (_sdk.BuySkin(Skin.ToString(), _costValue))
      {
        SwitchBetweenBtns(BoughtButton);
      }
    }

    public void SetSkin()
    {
      if (!_activeSkin && IsBoughtSkin())
      {
        _container.ChangeItemsToNotActive();
        SetActiveItem(true);
        _sdk.ChangeSkin(Skin.ToString());
      }
    }

    private bool IsBoughtSkin() => 
      Skin == Skins.Skin1 || _allShipsBought || _progress.UserData.FindSkin(Skin.ToString()) != -1;

    private bool HaveToActive() => 
      _progress.UserData.Skin == Skin.ToString() || Skin == Skins.Skin1 && string.IsNullOrEmpty(_progress.UserData.Skin);

    private void CheckForEnoughBonuses()
    {
      SwitchBetweenBtns(_progress.UserData.Bonuses >= _costValue ? BuyButton : LockedButton);
    }

    private void SwitchBetweenBtns(GameObject button)
    {
      BuyButton.SetActive(BuyButton == button);
      BoughtButton.SetActive(BoughtButton == button);
      LockedButton.SetActive(LockedButton == button);
    }
  }
}