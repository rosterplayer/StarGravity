using StarGravity.Infrastructure.AssetManagement;
using StarGravity.Infrastructure.Factories;
using StarGravity.Infrastructure.Services.Progress;
using StarGravity.Infrastructure.Services.SDK;

namespace StarGravity.Infrastructure.Services.Tutorial
{
  public class TutorialService
  {
    private readonly ProgressService _progress;
    private readonly PopupFactory _popupFactory;
    private readonly GamePrefabs _gamePrefabs;
    private ISDKWrapper _sdk;

    public TutorialService(ProgressService progress, PopupFactory popupFactory, GamePrefabs gamePrefabs, ISDKWrapper sdk)
    {
      _sdk = sdk;
      _progress = progress;
      _popupFactory = popupFactory;
      _gamePrefabs = gamePrefabs;
    }

    public void ShowTutorial(int forShip)
    {
      if (_progress.UserData.Tutorials[forShip] != 0 || !IsTutorialExist(forShip))
        return;
      
      _popupFactory.Create(_gamePrefabs.TutorialPrefabs[forShip]);
      _progress.UserData.Tutorials[forShip] = 1;
      _sdk.SaveProgress();
    }

    private bool IsTutorialExist(int tutorialKey) => 
      _gamePrefabs.TutorialPrefabs.Length > tutorialKey && _gamePrefabs.TutorialPrefabs[tutorialKey] != null;
  }
}