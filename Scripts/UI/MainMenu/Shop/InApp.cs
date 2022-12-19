using StarGravity.Infrastructure.Services.InApp;
using UnityEngine;
using VContainer;

namespace StarGravity.UI.MainMenu.Shop
{
  public class InApp : MonoBehaviour
  {
    [SerializeField]
    private string _id;

    [SerializeField]
    [Tooltip("If item can be purchased only once")]
    private bool _permanent;

    private IInAppService _inAppService;

    [Inject]
    public void Construct(IInAppService inAppService)
    {
      _inAppService = inAppService;
      _inAppService.OnPurchaseSuccess += OnPurchaseSuccess;
      
      if (_permanent)
        _inAppService.PurchasesUpdated += CheckIsPurchasedAlready;
    }

    private void Start()
    {
      CheckIsPurchasedAlready();
    }

    public void Purchase()
    {
      _inAppService.Purchase(_id);
    }

    private void OnPurchaseSuccess(string purchaseId)
    {
      if (!_permanent) 
        return;
      
      gameObject.SetActive(false);
    }

    private void CheckIsPurchasedAlready()
    {
      if (_inAppService.FindInPurchasedItems(_id) != null) 
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
      _inAppService.OnPurchaseSuccess -= OnPurchaseSuccess;
      
      if (_permanent)
        _inAppService.PurchasesUpdated -= CheckIsPurchasedAlready;
    }
  }
}