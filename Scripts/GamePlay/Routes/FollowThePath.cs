using StarGravity.GamePlay.Utilities;
using UnityEngine;

namespace StarGravity.GamePlay.Routes
{
  public class FollowThePath : MonoBehaviour
  {
    [HideInInspector] public Transform[] path; //path points which passes the 'Ship' 
    [HideInInspector] public float speed;
    [HideInInspector] public bool rotationByPath; //whether 'Ship' rotates in path direction or not
    [HideInInspector] public bool loop; //if loop is true, 'Ship' returns to the path starting point after completing the path
    [HideInInspector] public bool movingIsActive; //whether 'Ship' moves or not

    private float _currentPathPercent; //current percentage of completing the path
    private Vector3[] _pathPositions; //path points in vector3

    //setting path parameters for the 'Enemy' and sending the 'Enemy' to the path starting point
    public void SetPath()
    {
      _currentPathPercent = 0;
      _pathPositions = new Vector3[path.Length]; //transform path points to vector3
      for (int i = 0; i < _pathPositions.Length; i++)
      {
        _pathPositions[i] = path[i].position;
      }

      transform.position = NewPositionByPath(_pathPositions); //sending the enemy to the path starting point
      if (!rotationByPath)
        transform.rotation = Quaternion.identity;
      movingIsActive = true;
    }

    private void Update()
    {
      if (movingIsActive)
      {
        _currentPathPercent += speed / 100 * Time.deltaTime; //every update calculating current path percentage according to the defined speed

        transform.position = NewPositionByPath(_pathPositions); //moving the 'Ship' to the path position, calculated in method NewPositionByPath
        if (rotationByPath) //rotating the 'Ship' in path direction, if set 'rotationByPath'
        {
          transform.right = _pathPositions.CreatePoints().Interpolate(_currentPathPercent + 0.01f) - transform.position;
          transform.Rotate(Vector3.forward * 90);
        }

        if (_currentPathPercent > 1) //when the path is complete
        {
          if (loop) //when loop is set, moving to the path starting point; if not, destroying or deactivating the 'Enemy'
            _currentPathPercent = 0;
          else
          {
            Destroy(gameObject);
          }
        }
      }
    }

    private Vector3 NewPositionByPath(Vector3[] pathPos) => 
      pathPos.CreatePoints().Interpolate(_currentPathPercent);
  }
}