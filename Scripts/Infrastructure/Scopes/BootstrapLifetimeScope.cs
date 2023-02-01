using StarGravity.Data;
using StarGravity.Infrastructure.AssetManagement;
using StarGravity.Infrastructure.Factories;
using StarGravity.Infrastructure.Services;
using StarGravity.Infrastructure.Services.Ad;
using StarGravity.Infrastructure.Services.Localization;
using StarGravity.Infrastructure.Services.Progress;
using StarGravity.Infrastructure.Services.SDK;
using StarGravity.Infrastructure.Services.Sound;
using StarGravity.Infrastructure.Services.StaticData;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace StarGravity.Infrastructure.Scopes
{
    public class BootstrapLifetimeScope : LifetimeScope
    {
        public GameParameters GameParameters;
        public GamePrefabs Prefabs;
        public GameObject SDKListenerPrefab;
        public AudioPlayer AudioPlayer;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(GameParameters);
            builder.RegisterInstance(Prefabs);
            builder.Register<ICollectableSequenceDataProvider, CollectableSequenceDataProvider>(Lifetime.Singleton);
            RegisterSoundService(builder);
            Instantiate(SDKListenerPrefab).name = "YandexSDK";
            builder.Register<ILocalizationService, LocalizationService>(Lifetime.Singleton);
            builder.Register<IProgressService, ProgressService>(Lifetime.Singleton);
            builder.Register<ISDKWrapper, YandexPlatformSDKWrapper>(Lifetime.Singleton);
            builder.Register<IAdService, AdService>(Lifetime.Singleton);
            builder.RegisterEntryPoint<Bootstrapper>();
        }

        private void RegisterSoundService(IContainerBuilder builder)
        {
            builder.RegisterComponentInNewPrefab(AudioPlayer, Lifetime.Singleton);
            builder.Register<SoundService>(Lifetime.Singleton);
        }
    }
}
