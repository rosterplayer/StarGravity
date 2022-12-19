using StarGravity.GamePlay.Utilities;
using UnityEngine;

namespace StarGravity.GamePlay.Routes
{
  public class ShipRoute : MonoBehaviour
  { 
    [Tooltip("Ship prefab")]
    public GameObject Ship;

    [Tooltip("path passage speed")]
    public float Speed;

    [Tooltip("points of the path. delete or add elements to the list if you want to change the number of the points")]
    public Transform[] PathPoints;

    [Tooltip("whether 'Ship' rotates in path passage direction")]
    public bool RotationByPath;

    [Tooltip("if loop is activated, after completing the path 'Ship' will return to the starting point")]
    public bool Loop;

    [Tooltip("color of the path in the Editor")]
    public Color pathColor = Color.yellow;

    private void Start()
    {
        CreateEnemyWave(); 
    }

    private void CreateEnemyWave() //depending on chosed parameters generating enemies and defining their parameters
    {
        GameObject newShip = Instantiate(Ship, Ship.transform.position, Quaternion.identity);
        FollowThePath followComponent = newShip.GetComponent<FollowThePath>(); 
        followComponent.path = PathPoints;         
        followComponent.speed = Speed;        
        followComponent.rotationByPath = RotationByPath;
        followComponent.loop = Loop;
        followComponent.SetPath(); 

        newShip.SetActive(true);
        
        if (!Loop)
            Destroy(gameObject); 
    }

    private void OnDrawGizmos()  
    {
        DrawPath(PathPoints);  
    }

    private void DrawPath(Transform[] path) //drawing the path in the Editor
    {
        Vector3[] pathPositions = new Vector3[path.Length];
        for (int i = 0; i < path.Length; i++)
        {
            pathPositions[i] = path[i].position;
        }
        Vector3[] newPathPositions = pathPositions.CreatePoints();
        Vector3 previousPositions = newPathPositions.Interpolate(0);
        Gizmos.color = pathColor;
        int smoothAmount = path.Length * 20;
        for (int i = 1; i <= smoothAmount; i++)
        {
            float t = (float)i / smoothAmount;
            Vector3 currentPositions = newPathPositions.Interpolate(t);
            Gizmos.DrawLine(currentPositions, previousPositions);
            previousPositions = currentPositions;
        }
    }
  }
}