using StarGravity.GamePlay;
using StarGravity.GamePlay.Planets;
using StarGravity.GamePlay.Player.Perks;
using StarGravity.GamePlay.Routes;
using StarGravity.Infrastructure.AssetManagement;
using StarGravity.Infrastructure.Factories;
using StarGravity.Infrastructure.Services.Input;
using StarGravity.Infrastructure.Services.Progress;
using StarGravity.Infrastructure.Services.Tutorial;
using StarGravity.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace StarGravity.Infrastructure.Scopes
{
    public class GameLifetimeScope : LifetimeScope
    {
        public UFOSpawner UfoSpawner;
        public DebugLog DebugLog;
        
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterMainCanvas(builder);
            builder.RegisterInstance(DebugLog);
            builder.Register<TutorialService>(Lifetime.Scoped);
            builder.RegisterComponentInNewPrefab(UfoSpawner, Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<InputService>().As<IInputService>();
            RegisterCaptureBeamFactory(builder);
            builder.Register<PlayerShipFactory>(Lifetime.Scoped);
            builder.Register<GameObjectFactory>(Lifetime.Scoped);
            builder.Register<PopupFactory>(Lifetime.Scoped);
            builder.Register<PlanetSpawner>(Lifetime.Scoped);
            builder.Register<GameLevelProgressService>(Lifetime.Scoped);

            builder.RegisterEntryPoint<Game>(Lifetime.Scoped);
        }

        private static void RegisterMainCanvas(IContainerBuilder builder)
        {
            var canvas = FindObjectOfType<Canvas>();
            builder.RegisterInstance(canvas);
        }

        private static void RegisterCaptureBeamFactory(IContainerBuilder builder)
        {
            builder.RegisterFactory<Transform, CaptureBeam>(container =>
            {
                return parent =>
                {
                    var beamPrefab = container.Resolve<GamePrefabs>().CaptureBeam;
                    return container.Instantiate(beamPrefab, parent).GetComponent<CaptureBeam>();
                };
            }, Lifetime.Scoped);
        }
    }
}
