using System;
using System.Collections.Generic;
using System.Linq;
using StarGravity.GamePlay.Interactables;
using UnityEngine;

namespace StarGravity.GamePlay.Player.Perks
{
  public static class Magnet
  {
    private const int CaptureDistance = 4;

    public static List<GameObject> GetBonusesForCapture(Vector2 magnetPosition, List<GameObject> bonuses) => 
      bonuses.Where(bonus => Vector2.Distance(bonus.transform.position, magnetPosition) < CaptureDistance).ToList();

    public static void Capture(GameObject magnet, List<GameObject> bonusesForCapture, Action onCaptured)
    {
      foreach (GameObject bonus in bonusesForCapture)
      {
        bonus.GetComponent<Bonus>().MoveTo(magnet, onCaptured);
      }
    }

    public static void Capture(GameObject magnet, GameObject bonus, Action onCaptured)
    {
      bonus.GetComponent<Bonus>().MoveTo(magnet, onCaptured);
    }
  }
}