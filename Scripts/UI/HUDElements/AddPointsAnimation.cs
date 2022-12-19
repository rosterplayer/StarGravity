using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using StarGravity.UI.MainMenu;
using TMPro;
using UnityEngine;

namespace StarGravity.UI.HUDElements
{
  public class AddPointsAnimation : MonoBehaviour
  {
    public Vector2 Offset;
    public float Duration;
    public TextMeshProUGUI Text;
    public RectTransform Transform;
    private Vector3 _originPosition;
    private Color _solidColor;
    private Color _transparentColor;
    private TweenerCore<Color, Color, ColorOptions> _pointsFadeAnimation;
    private TweenerCore<Vector3, Vector3, VectorOptions> _pointsMoveAnimation;
    private int _addingPoints;

    private void Start()
    {
      _originPosition = Transform.localPosition;
      _solidColor = Text.color;
      _transparentColor = new Color(_solidColor.r, _solidColor.g, _solidColor.b, 1f);
    }

    public void Play()
    {
      Reset();
      _pointsFadeAnimation = Text.DOFade(0f, Duration);
      _pointsMoveAnimation = Transform.DOLocalMove(_originPosition + (Vector3)Offset, Duration);
    }

    public void SetPoints(int points)
    {
      if (_pointsFadeAnimation.IsActive() || _pointsMoveAnimation.IsActive())
        _addingPoints += points;
      else
        _addingPoints = points;
      
      Text.text = $"+{_addingPoints}";
    }

    private void Reset()
    {
      Transform.localPosition = _originPosition;
      Text.color = _solidColor;
      _pointsFadeAnimation.Kill();
      _pointsMoveAnimation.Kill();
    }
  }
}