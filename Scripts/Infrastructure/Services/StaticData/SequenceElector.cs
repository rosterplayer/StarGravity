using StarGravity.Data;
using UnityEngine;

namespace StarGravity.Infrastructure.Services.StaticData
{
  public class SequenceElector
  {
    private readonly ICollectableSequenceDataProvider _sequenceDataProvider;

    public SequenceElector(ICollectableSequenceDataProvider sequenceDataProvider)
    {
      _sequenceDataProvider = sequenceDataProvider;
    }

    public GameObject Elect(Vector2 leftPlanet, Vector2 star, Vector2 rightPlanet)
    {
      var prefabs = _sequenceDataProvider.GetPrefabs(OnScreen(leftPlanet), OnScreen(star), OnScreen(rightPlanet));
      return prefabs[Random.Range(0, prefabs.Count)];
    }

    public GameObject Elect(string prefabName) => 
      _sequenceDataProvider.GetPrefabByName(prefabName);

    private PositionOnScreen OnScreen(Vector2 position)
    {
      var bottomLeftPoint = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
      var topLeftPoint = Camera.main.ViewportToWorldPoint(new Vector2(0, 1));

      float height = topLeftPoint.y - bottomLeftPoint.y;
      float positionToHeight = position.y - bottomLeftPoint.y;
      
      if (positionToHeight > height * 3 / 5)
        return PositionOnScreen.Top;
      if (positionToHeight < height * 2 / 5)
        return PositionOnScreen.Bottom;
      return PositionOnScreen.Mid;
    } 
  }
}