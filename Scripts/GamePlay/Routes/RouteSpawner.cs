using UnityEngine;

namespace StarGravity.GamePlay.Routes
{
  public abstract class RouteSpawner : MonoBehaviour
  {
    public GameObject[] Routes;

    protected void CreateRoute()
    {
      GameObject route = Routes[Random.Range(0, Routes.Length)];
      Instantiate(route, route.transform.position, Quaternion.identity);
    }
  }
}