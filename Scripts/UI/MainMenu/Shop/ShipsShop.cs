using StarGravity.Infrastructure.Services.InApp;
using StarGravity.Infrastructure.Services.Progress;
using VContainer;

namespace StarGravity.UI.MainMenu.Shop
{
  public class ShipsShop : Shop
  {
    private IInAppService _inAppService;

    [Inject]
    public void Construct(IProgressService progress, IInAppService inAppService)
    {
      base.Construct(progress);
      _inAppService = inAppService;
      _inAppService.GetPurchases();
    }
  }
}