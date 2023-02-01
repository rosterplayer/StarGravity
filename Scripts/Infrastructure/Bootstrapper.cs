using StarGravity.Infrastructure.Services.Progress;
using StarGravity.Infrastructure.Services.SDK;
using StarGravity.Infrastructure.Services.StaticData;
using VContainer.Unity;

namespace StarGravity.Infrastructure
{
  public class Bootstrapper : IInitializable
  {
    private readonly ISDKWrapper _sdkWrapper;
    private readonly IProgressService _progress;
    private readonly ICollectableSequenceDataProvider _collectableSequenceDataProvider;

    public Bootstrapper(ISDKWrapper sdkWrapper, IProgressService progress, ICollectableSequenceDataProvider collectableSequenceDataProvider)
    {
      _sdkWrapper = sdkWrapper;
      _progress = progress;
      _collectableSequenceDataProvider = collectableSequenceDataProvider;
    }
    public void Initialize()
    {
      _collectableSequenceDataProvider.LoadData();
      _progress.Load();
      _sdkWrapper.Subscribe();
    }
  }
}