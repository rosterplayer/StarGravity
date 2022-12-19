using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace StarGravity.UI
{
  public class DebugLog : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI _text;
    private Tween _delayedCall;

    public void Append(string text)
    {
      _text.text += $"\n{text}";
      if (_delayedCall.IsActive())
      {
        _delayedCall.Kill();
      }
      _delayedCall = DOVirtual.DelayedCall(6f, () => _text.text = "");
    }
  }
}