using System.Collections;
using UnityEngine;

namespace StarGravity.GamePlay.Player
{
  public class FxDestruction : MonoBehaviour
  {
    public float destructionTime;

    private void OnEnable()
    {
      StartCoroutine(Destruction()); //launching the timer of destruction
    }

    IEnumerator Destruction() //wait for the estimated time, and destroying or deactivating the object
    {
      yield return new WaitForSeconds(destructionTime); 
      Destroy(gameObject);
    }  
  }
}