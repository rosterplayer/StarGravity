using System;
using StarGravity.Data;

namespace StarGravity.Infrastructure.Services.InApp
{
  public interface IInAppService
  {
    event Action PurchasesUpdated;
    event Action<string> OnPurchaseSuccess;
    void GetPurchases();
    void Purchase(string inAppId);
    Purchase FindInPurchasedItems(string purchaseId);
  }
}