using StarGravity.Infrastructure.AssetManagement;
using StarGravity.Infrastructure.Factories;
using StarGravity.Infrastructure.Services.Progress;
using StarGravity.Infrastructure.Services.SDK;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace StarGravity.UI.MainMenu
{
  public class Menu : MonoBehaviour
  {
    public TextMeshProUGUI BestScore;

    private ISDKWrapper _sdk;
    private IProgressService _progress;
    private IPopupFactory _popupFactory;
    private GamePrefabs _prefabs;

    [Inject]
    public void Construct(ISDKWrapper sdkWrapper, IProgressService progress, IPopupFactory popupFactory, GamePrefabs prefabs)
    {
      _prefabs = prefabs;
      _popupFactory = popupFactory;
      _sdk = sdkWrapper;
      _progress = progress;
      
      _sdk.OnUserDataReceived += ChangeText;
      _sdk.ShowAuthInvite += ShowAuthPopup;
    }
    
    private void Start()
    {
      ChangeText();
    }

    public void StartGame()
    {
      SceneManager.LoadScene("Space", LoadSceneMode.Single);
    }

    private void ChangeText()
    {
      BestScore.text = $"{_progress.UserData.BestScore}";
    }

    private void ShowAuthPopup()
    {
      _popupFactory.Create(_prefabs.AuthPopup);
    }

    private void OnDestroy()
    {
      _sdk.OnUserDataReceived -= ChangeText;
      _sdk.ShowAuthInvite -= ShowAuthPopup;
    }
  }
}