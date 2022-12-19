using StarGravity.Infrastructure.Services.SDK;
using StarGravity.UI.Essentials.Popups;
using UnityEngine;
using VContainer;

namespace StarGravity.UI.MainMenu
{
  public class AuthenticationPopup : Popup
  {
    private ISDKWrapper _sdk;

    [Inject]
    public void Construct(ISDKWrapper sdk)
    {
      _sdk = sdk;
    }

    public void Authenticate()
    {
      _sdk.Authenticate();
      Close();
    }
  }
}