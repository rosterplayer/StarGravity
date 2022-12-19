using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

namespace StarGravity.UI.Essentials
{
  public class DynamicLocalizationText : MonoBehaviour
  {
    [SerializeField] private LocalizedString _localizedString;
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

    private void OnEnable()
    {
      _localizedString.StringChanged += UpdateText;
    }

    private void OnDisable()
    {
      _localizedString.StringChanged -= UpdateText;
    }

    public void ChangeText(IList<object> args)
    {
      _localizedString.Arguments = args;
      _localizedString.RefreshString();
    }

    private void UpdateText(string value) => 
      _textMeshProUGUI.text = value;
  }
}