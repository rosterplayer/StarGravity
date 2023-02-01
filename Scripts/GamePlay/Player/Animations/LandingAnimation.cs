using System;
using DG.Tweening;
using UnityEngine;

namespace StarGravity.GamePlay.Player.Animations
{
  public class LandingAnimation : MonoBehaviour
  {
    public void StartAnimation(Vector2 planetPosition, float planetRotation, Action onAnimationFinished)
    {
      Vector2 planetOrbitDirection = DirectionOnPlanetOrbit(
        transform.position, 
        planetPosition, 
        planetRotation
      );
      // angle between moving direction and direction on orbit
      float angle = Vector2.Angle(transform.up, planetOrbitDirection);
      float duration = GetAnimationDuration(angle);
      Quaternion rotation = Quaternion.LookRotation(Vector3.forward, planetOrbitDirection);

      /*if (angle > 45)
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
          });*/


      transform.DORotate(rotation.eulerAngles, duration)
        .OnComplete(() =>
        {
          onAnimationFinished?.Invoke();
        });
    }
    
    private Vector2 DirectionOnPlanetOrbit(Vector2 ship, Vector2 planet, float planetRotation) => 
      (Vector2.Perpendicular(ship - planet) * planetRotation).normalized;

    private float GetAnimationDuration(float angle)
    {
      if (angle < 45)
        return 0.1f;
      if (angle < 90)
        return 0.2f;

      return 0.3f;
    }
  }
}