using System.Collections.Generic;
using StarGravity.Infrastructure.Services.Progress;
using StarGravity.UI.Essentials;
using StarGravity.UI.Essentials.Popups;
using TMPro;
using UnityEngine.SceneManagement;
using VContainer;

namespace StarGravity.UI
{
  public class GameOverPopup : Popup
  {
    public TextMeshProUGUI Points;
    public DynamicLocalizationText HighScore;
    public DynamicLocalizationText DetailsSystems;
    public DynamicLocalizationText DetailsStars;
    public DynamicLocalizationText DetailsBonuses;
    
    private IGameLevelProgressService _levelProgressService;
    private IProgressService _progress;

    [Inject]
    public void Construct(IGameLevelProgressService levelProgressService, IProgressService progress)
    {
      _progress = progress;
      _levelProgressService = levelProgressService;
    }

    private void Start()
    {
      SetPoints();
    }

    private void SetPoints()
    {
      DetailsSystems.ChangeText(new List<object>() { _levelProgressService.LevelProgress.Systems });
      DetailsStars.ChangeText(new List<object>() { _levelProgressService.LevelProgress.BonusStars });
      DetailsBonuses.ChangeText(new List<object>() { _levelProgressService.LevelProgress.ShipBonusesCollected });
      Points.text = $"{_levelProgressService.LevelProgress.TotalPoints}";
      HighScore.ChangeText(new List<object>() { _progress.UserData.BestScore });
    }
    
    public void ReloadScene()
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMenu()
    {
      SceneManager.LoadScene("Menu");
    }
  }
}