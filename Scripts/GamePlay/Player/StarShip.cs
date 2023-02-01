using System;
using DG.Tweening;
using Leopotam.Ecs;
using StarGravity.GamePlay.Interactables.Bonuses;
using StarGravity.GamePlay.Planets;
using StarGravity.GamePlay.Player.Animations;
using StarGravity.GamePlay.Player.Components;
using StarGravity.GamePlay.Player.Perks;
using StarGravity.GamePlay.Utilities;
using StarGravity.Infrastructure.Factories;
using StarGravity.Infrastructure.Services.Progress;
using UnityEngine;
using VContainer;
using Voody.UniLeo;

namespace StarGravity.GamePlay.Player
{
    public class StarShip : MonoBehaviour
    {
        private const float MagnetTime = 10;

        public ShipPerk ShipPerk;
        [SerializeField] private ConvertToEntity _convertToEntity;
        [SerializeField] private FlashingAnimation _flashingAnimation;
        [SerializeField] private LandingAnimation _landingAnimation;
        [SerializeField] private Health _health;
        
        private Tweener _magnetTweener;
        private IGameObjectFactory _objectFactory;
        private IGameLevelProgressService _levelProgressService;

        public event Action UseAbility;
        public event Action UpPressed;
        public event Action DownPressed;
        public event Action<float> OnMagnetActivated;

        [Inject]
        public void Construct(IGameObjectFactory gameObjectFactory, IGameLevelProgressService levelProgressService)
        {
            _objectFactory = gameObjectFactory;
            _levelProgressService = levelProgressService;
        }

        public void OnRespawn()
        {
            _flashingAnimation.StartAnimation();
        }

        public void OnUseAbility() => 
            UseAbility?.Invoke();
        
        public void OnUpPressed() => 
            UpPressed?.Invoke();
        
        public void OnDownPressed() => 
            DownPressed?.Invoke();

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.collider.CompareTag("Wheel")) 
                return;

            if (!_convertToEntity.TryGetEcsEntity(out EcsEntity entity))
                return;

            entity.Get<StartLandingEvent>();
            ref var shipState = ref entity.Get<ShipState>();
            shipState.State = PlayerState.OnLanding;

            _flashingAnimation.StopAnimation();

            _landingAnimation.StartAnimation(
                collision.gameObject.transform.position, 
                collision.gameObject.GetComponent<Rotator>().CurrentRotation, 
                () =>
                {
                    ref var landedOn = ref entity.Get<LandedOn>();
                    landedOn.Planet = collision.gameObject;
                }
            ); 
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Bonus"))
            {
                _levelProgressService.OnStarBonusCollected(other.gameObject.GetComponent<Bonus>().Points);
            }
            else if (other.CompareTag("Health"))
            {
                _health.SetConstantHealth();
                _levelProgressService.OnShipBonusCollected();
            }
            else if (other.CompareTag("Magnet"))
            {
                ActivateMagnet();
                _levelProgressService.OnShipBonusCollected();
            }
            else if (other.CompareTag("Asteroid")
                     && !ActiveShield()) 
                SetCrashEvent();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("GravityZone"))
                SetGravityComponent(other);
            if (other.CompareTag("DeathZone")) 
                SetCrashEvent();
        }

        private void SetGravityComponent(Collider2D other)
        {
            if (!_convertToEntity.TryGetEcsEntity(out EcsEntity entity))
                return;

            ref var gravity = ref entity.Get<InGravityField>();
            gravity.GravityFieldCenter = other.transform.position;
            gravity.GravityFieldRadius = other.GetComponent<CircleCollider2D>().radius;
        }

        private void SetCrashEvent()
        {
            if (!_convertToEntity.TryGetEcsEntity(out EcsEntity entity))
                return;
            
            entity.Get<CrashedEvent>();
        }

        private bool ActiveShield() => 
            ShipPerk != null && ShipPerk is Shield { IsActive: true };
        
        private void ActivateMagnet()
        {
            OnMagnetActivated?.Invoke(MagnetTime);
            _magnetTweener = DOVirtual.Int(0, 10, MagnetTime, value =>
            {
                Magnet.Capture(gameObject, Magnet.GetBonusesForCapture(transform.position, _objectFactory.Bonuses), null);
            });
        }

        private void OnDestroy()
        {
            _magnetTweener?.Kill();
        }
    }
}