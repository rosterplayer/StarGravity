using System;
using System.Linq;
using StarGravity.Data;
using StarGravity.Infrastructure.Services.SDK;
using UnityEngine;

namespace StarGravity.Infrastructure.Services.InApp
{
  public class InAppService : IInAppService, IDisposable
  {
    private Purchase[] _purchases = {};

    private readonly ISDKWrapper _sdk;

    public event Action PurchasesUpdated;
    public event Action<string> OnPurchaseSuccess;

    public InAppService(ISDKWrapper sdk)
    {
      _sdk = sdk;
      Subscribe();
    }

    public void GetPurchases()
    {
      _sdk.GetPurchases();
    }

    public void Purchase(string inAppId)
    {
      _sdk.Purchase(inAppId);
    }

    public Purchase FindInPurchasedItems(string purchaseId) =>
      _purchases.FirstOrDefault(x => x.productID == purchaseId);

    private void Subscribe()
    {
      _sdk.OnGetPurchases += OnGetPurchases;
      _sdk.OnPurchaseSuccess += OnPurchased;
    }

    private void Unsubscribe()
    {
      _sdk.OnGetPurchases -= OnGetPurchases;
      _sdk.OnPurchaseSuccess -= OnPurchased;
    }

    private void OnGetPurchases(PurchasesCollection purchasesCollection)
    {
      _purchases = purchasesCollection.purchases;
      PurchasesUpdated?.Invoke();
    }

    private void OnPurchased(string purchaseId)
    {
      OnPurchaseSuccess?.Invoke(purchaseId);
    }

    public void Dispose()
    {
      Unsubscribe();
    }
  }
}