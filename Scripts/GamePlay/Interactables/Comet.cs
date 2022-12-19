using System.Collections;
using UnityEngine;

namespace StarGravity.GamePlay.Interactables
{
    public class Comet : MonoBehaviour {
        //Time that takes in activate the comet after generation
        [Range(0f, 30.0f)]
        public float SpawnTime = 4f;
        
        //This speed value to move the comet
        [Range(35f, 150f)]
        public float Speed = 35f;

        private float _currentSpawnTime;
        private float _currentSpeed;

        //If not activated, the comet does not move
        private bool _activated;

        //Define if the spawnTime and the speed should be randomized or not at generation
        public bool randomize = true;
        private Camera _mainCamera;

        void Start ()
        {
            _mainCamera = Camera.main;
            
            if (!randomize)
            {
                _currentSpawnTime = SpawnTime;
                _currentSpeed = Speed;
            }
            Generate();
        }

        private void Generate()
        {
            //Deactivate the comet
            Activate(false);
            //Randomize spawn time and speed
            if (randomize)
            {
                _currentSpawnTime = Random.Range(0.3f, SpawnTime);
                _currentSpeed = Random.Range(35f, Speed);
            }
            //Wait for currentSpawnTime to reactivate the comet
            StartCoroutine(WaitToActivate(_currentSpawnTime));
        }

        IEnumerator WaitToActivate(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            Activate(true);
        }

        //Activate or deactivate the shooting star movement
        private void Activate(bool activate)
        {
            _activated = activate;
            if (_activated)
            {
                //Once activated, the first action is to give the shooting star a new position
                Vector3 newPosition = _mainCamera.ViewportToWorldPoint(new Vector3(Random.Range(-0.5f, 1f), (Random.Range(0, 100) < 50 ? -0.5f : 1.5f), 0f));
            
                transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
                //It defines the point to the shooting star will be pointing
                Vector3 forwardDirection = new Vector3(_mainCamera.transform.position.x, _mainCamera.transform.position.y, 0f) - transform.position;
                //Force the forwardDirection to don't change the position in the z axis
                transform.forward = new Vector3(forwardDirection.x, forwardDirection.y, 0f);
            }
        }
    
        void Update () {
            //If is not activated, don't update
            if (!_activated) return;
            transform.position += transform.forward * _currentSpeed * Time.deltaTime;
            //Ask if the shooting star has reached the limit to be regenerated
            Rect rect = new Rect(-1f, -1f, 3f, 3f);
            if (!rect.Contains(_mainCamera.WorldToViewportPoint(transform.position)))
            {
                Generate();
            }
        }
    }
}
