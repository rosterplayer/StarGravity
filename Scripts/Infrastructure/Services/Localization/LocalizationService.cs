using System.Threading.Tasks;
using UnityEngine.Localization.Settings;

namespace StarGravity.Infrastructure.Services.Localization
{
  public class LocalizationService : ILocalizationService
  {
    public async void ChangeLocale(string language)
    {
      await SetLocale(LocalizationStringToLocaleId(language));
    }

    private async Task SetLocale(int localeId)
    {
      await LocalizationSettings.InitializationOperation.Task;
      LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeId];
    }

    private static int LocalizationStringToLocaleId(string language)
    {
      switch (language)
      {
        case "ru":
        case "be":
        case "kk":
        case "uk":
        case "uz":
          return 1;
        case "tr":
          return 2;
        default:
          return 0;
      }
    }
  }
}