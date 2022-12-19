using UnityEngine;
using Random = UnityEngine.Random;

namespace StarGravity.GamePlay.Planets
{
    public class Rotator : MonoBehaviour
    {
        public float MaxRotateSpeed;
        public float MinRotateSpeed;
        public bool RotateOnStart;
        [Tooltip("Set whether the rotation speed should be randomized or not")]
        public bool Randomize;
        
        private bool _rotate;
        private float _currentRotationSpeed;
        private float _angularDirection;

        public float CurrentRotation => _angularDirection * _currentRotationSpeed;

        private void Start()
        {
            AssignRotationSpeed();
            
            if (!RotateOnStart) 
                return;
            
            AssignAngularDirection(0);
            _rotate = true;
        }

        private void Update()
        {
            if (_rotate)
                transform.Rotate(0, 0, _angularDirection * _currentRotationSpeed * Time.deltaTime);
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.collider.CompareTag("Player"))
            {
                var rotationDirection = GetRotationDirection(col.gameObject.transform.up, col.GetContact(0).point); 
                AssignAngularDirection(rotationDirection);
                _rotate = true;
            }
        }

        private void AssignRotationSpeed() => 
            _currentRotationSpeed = Randomize ? Random.Range(MinRotateSpeed, MaxRotateSpeed) : MaxRotateSpeed;

        private void AssignAngularDirection(float angularDirection)
        {
            if (angularDirection == 0)
                _angularDirection = Random.Range(0, 100) < 50 ? -1f : 1f;
            else
                _angularDirection = angularDirection;
        }

        private float GetRotationDirection(Vector3 playerShipDirection, Vector2 collisionPoint)
        {
            Vector2 r = collisionPoint - (Vector2)transform.position;
            float angle = Vector2.Angle(Vector2.right, r);
            angle = AngleTo360(collisionPoint, angle);
            Vector2 addition = r + ((Vector2)playerShipDirection);
            float angle2 = Vector2.Angle((Vector2.right), addition);
            angle2 = AngleTo360(collisionPoint, angle2);
            
            if (angle2 > angle)
                return 1f;
            else
                return -1f;
        }

        private float AngleTo360(Vector2 collisionPoint, float angle) => 
            collisionPoint.y < transform.position.y ? (360 - angle) : (angle);
    }
}