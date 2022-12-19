using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using StarGravity.GamePlay.Interactables;
using StarGravity.GamePlay.Planets;
using StarGravity.GamePlay.Player.Perks;
using StarGravity.Infrastructure.Factories;
using StarGravity.Infrastructure.Services.Input;
using StarGravity.Infrastructure.Services.Sound;
using StarGravity.UI;
using UnityEngine;
using VContainer;

namespace StarGravity.GamePlay.Player
{
    public class StarShip : MonoBehaviour
    {
        private const float MagnetTime = 10;
        
        public PlayerState PlayerState;
        public GameObject DestructionFX;
        public ShipPerk ShipPerk;

        [SerializeField]
        private float _gravityMinForce = 8;
        [SerializeField]
        private float _horizontalSpeed;
        [SerializeField]
        private GameObject _reachedPlanet;
        [SerializeField]
        private Vector2 _startPosition;

        // for testing in browser window
        [Inject] private DebugLog _debugLog;

        private Rigidbody2D _body;
        private Health _health;
        private IInputService _inputService;
        private SoundService _soundService;
        private SpriteRenderer _spriteRenderer;
        private GameObjectFactory _objectFactory;
        private CircleCollider2D _collider2D;
        
        private bool _initiated;
        private bool _untouchable;
        
        private Tweener _magnetTweener;
        private TweenerCore<Color, Color, ColorOptions> _flashing;
        private Destination _destinationReachedPlanet;

        public event Action<GameObject> PlanetReached;
        public event Action<int> BonusCollected;
        public event Action ShipBonusCollected;
        public event Action UseAbility;
        public event Action<float> OnMagnetActivated;

        [Inject]
        public void Construct(IInputService inputService, SoundService soundService, GameObjectFactory gameObjectFactory)
        {
            _inputService = inputService;
            _soundService = soundService;
            _objectFactory = gameObjectFactory;
        }

        public void Initialize(GameObject firstPlanet)
        {
            _reachedPlanet = firstPlanet;
            _initiated = true;
            _inputService.GainControl();
        }

        private void Start()
        {
            PlayerState = PlayerState.OnLevelStart;
            _body = GetComponent<Rigidbody2D>();
            _health = GetComponent<Health>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collider2D = GetComponent<CircleCollider2D>();
            _startPosition = transform.position;
        }

        private void Update()
        {
            if (!_initiated || _inputService.Paused)
                return;
            
            // on new game start or after respawn
            if (PlayerState == PlayerState.OnLevelStart) 
                FlyAfterRespawn();

            if (_inputService.PressInput) ChooseAction();

            if (_body.velocity != Vector2.zero)
                RotateShip();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.collider.CompareTag("Wheel")) 
                return;
            
            // reached destination planet, invoke event to move planets and backgrounds
            PlayerState = PlayerState.OnLanding;
            ActivateFlashingAnimation(false);
            _untouchable = false;
            _body.constraints = RigidbodyConstraints2D.FreezeAll;
            _reachedPlanet = collision.gameObject;
                
            _destinationReachedPlanet = collision.gameObject.GetComponent<Destination>();
            _destinationReachedPlanet.SwitchOffCollider();
            if (!_destinationReachedPlanet.Reached)
            {
                _destinationReachedPlanet.MakeReached();
                AnimateLanding(collision, () =>
                {
                    transform.SetParent(_reachedPlanet.transform);
                    PlayerState = PlayerState.OnPlanetOrbit;
                    PlanetReached?.Invoke(_reachedPlanet);
                });                    
            }
            else
            {
                AnimateLanding(collision, () =>
                {
                    transform.SetParent(_reachedPlanet.transform);
                    PlayerState = PlayerState.OnPlanetOrbit;
                });  
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Bonus"))
                BonusCollected?.Invoke(other.gameObject.GetComponent<Bonus>().Points);
            else if (other.CompareTag("Health"))
            {
                _health.SetConstantHealth();
                ShipBonusCollected?.Invoke();
            }
            else if (other.CompareTag("Magnet"))
            {
                ActivateMagnet();
                ShipBonusCollected?.Invoke();
            }
            else if (other.CompareTag("Asteroid")
                     && PlayerState != PlayerState.OnPlanetOrbit && PlayerState != PlayerState.OnLanding
                     && !_untouchable
                     && !ActiveShield()
                    ) 
                Destruction();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (_untouchable)
                return;
            
            if (other.CompareTag("GravityZone"))
            {
                Vector2 direction = other.transform.position - transform.position;
                float force = _gravityMinForce * other.GetComponent<CircleCollider2D>().radius / direction.magnitude;
                direction.Normalize();
                _body.AddForce(direction * force, ForceMode2D.Force);
            }
            else if (other.CompareTag("DeathZone"))
            {
                Destruction();
            }
        }

        private void FlyAfterRespawn()
        {
            _body.velocity = (_reachedPlanet.transform.position - transform.position).normalized * _horizontalSpeed;
            PlayerState = PlayerState.OnFLy;
        }

        private void ChooseAction()
        {
            switch (PlayerState)
            {
                case PlayerState.OnPlanetOrbit:
                    JumpFromOrbit();
                    break;
                case PlayerState.OnFLy:
                    UseAbility?.Invoke();
                    break;
            }
        }

        private void RotateShip()
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, _body.velocity.normalized);
        }

        private void AnimateLanding(Collision2D collisionWithPlanet, Action onAnimationFinished)
        {
            const float radiusCorrection = 1.3f;
            var planetPosition = (Vector2)collisionWithPlanet.gameObject.transform.position;
            var planetColliderRadius = collisionWithPlanet.gameObject.GetComponent<CircleCollider2D>().radius;
            var planetColliderScale = collisionWithPlanet.gameObject.transform.localScale;
            var dir = collisionWithPlanet.GetContact(0).point - planetPosition;
            Vector2 sourcePoint = planetPosition + dir.normalized * planetColliderRadius * planetColliderScale.x + dir.normalized * _collider2D.radius * radiusCorrection;
            Vector2 planetOrbitDirection = DirectionOnPlanetOrbit(
                sourcePoint, 
                planetPosition, 
                collisionWithPlanet.gameObject.GetComponent<Rotator>().CurrentRotation
            );
            // angle between moving direction and direction on orbit
            float angle = Vector2.Angle(transform.up, planetOrbitDirection);
            float duration = GetAnimationDuration(angle);
            Sequence tweenSequence = DOTween.Sequence();
            if (angle > 45)
                tweenSequence.Append(
                    transform.DOMove(collisionWithPlanet.GetContact(0).point, duration / 2)
                    .OnComplete(() =>
                    {
                        transform.DOMove(sourcePoint, duration / 2);
                    })
                );
            else
                tweenSequence.Append(transform.DOMove(sourcePoint, duration));

            tweenSequence
                .Join(transform.DORotateQuaternion(Quaternion.LookRotation(Vector3.forward,planetOrbitDirection), duration))
                .OnComplete(() =>
                {
                    onAnimationFinished?.Invoke();
                });
        }

        private void OnBecameInvisible()
        {
            if (PlayerState == PlayerState.OnFLy)
                Respawn();
        }

        private void JumpFromOrbit()
        {
            _body.constraints = RigidbodyConstraints2D.None;
            transform.SetParent(null);
            PlayerState = PlayerState.OnFLy;
            _body.AddForce(transform.up * 12, ForceMode2D.Impulse);
            if (_destinationReachedPlanet != null) _destinationReachedPlanet.SwitchOnColliderWithDelay();
        }

        private float GetAnimationDuration(float angle)
        {
            if (angle < 45)
                return 0.1f;
            if (angle < 90)
                return 0.2f;

            return 0.3f;
        }

        private Vector2 DirectionOnPlanetOrbit(Vector2 ship, Vector2 planet, float planetRotation) => 
            (Vector2.Perpendicular(ship - planet) * planetRotation).normalized;

        private void Destruction()
        {
            Instantiate(DestructionFX, transform.position, Quaternion.identity); //generating destruction visual effect and destroying the 'Player' object
            _soundService.PlaySFX(SfxType.ShipCrash);
            Respawn();
        }

        private void Respawn()
        {
            if (_untouchable)
                return;

            transform.DOKill();
            _untouchable = true;
            ActivateFlashingAnimation(true);
            transform.position = _startPosition;
            _body.velocity = Vector2.zero;
            _body.angularVelocity = 0;
            PlayerState = PlayerState.OnLevelStart;
            _health.DecreaseHealth();
        }

        private bool ActiveShield() => 
            ShipPerk != null && ShipPerk is Shield { IsActive: true };

        private void ActivateFlashingAnimation(bool activate)
        {
            if (activate)
                _flashing = _spriteRenderer.DOFade(0, 0.3f).SetLoops(-1, LoopType.Yoyo);
            else
            {
                _flashing.Kill();
                _spriteRenderer.DOFade(1, 0.1f);
            }
        }

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
            _flashing?.Kill();
        }
    }
}