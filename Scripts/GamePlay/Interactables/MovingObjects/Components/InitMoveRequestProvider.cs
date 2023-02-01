using UnityEngine;
using Voody.UniLeo;

namespace StarGravity.GamePlay.Interactables.MovingObjects.Components
{
  [RequireComponent(typeof(InitMoveData))]
  public class InitMoveRequestProvider : MonoProvider<InitMoveRequest> {}
}