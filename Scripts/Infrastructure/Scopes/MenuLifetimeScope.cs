using StarGravity.Infrastructure.Factories;
using StarGravity.Infrastructure.Services.InApp;
using StarGravity.Infrastructure.Services.Sound;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace StarGravity.Infrastructure.Scopes
{
    public class MenuLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterMainCanvas(builder);
            builder.Register<PopupFactory>(Lifetime.Scoped);
            builder.Register<IInAppService, InAppService>(Lifetime.Scoped);
            builder.RegisterEntryPoint<MenuPresenter>();
        }
        
        private static void RegisterMainCanvas(IContainerBuilder builder)
        {
            var canvas = FindObjectOfType<Canvas>();
            builder.RegisterInstance(canvas);
        }
    }

    public class MenuPresenter : IStartable
    {
        private readonly SoundService _soundService;

        public MenuPresenter(SoundService soundService)
        {
            _soundService = soundService;
        }
        
        public void Start()
        {
            _soundService.PlayMusic(SceneType.Menu);
        }
    }
}
