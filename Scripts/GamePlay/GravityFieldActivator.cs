using UnityEngine;

namespace StarGravity.GamePlay
{
  public class GravityFieldActivator : MonoBehaviour
  {
    [SerializeField] private Collider2D _gravityCollider;
    private Vector3 _bottomLeftPoint;
    private Vector3 _topRightPoint;

    private void Awake()
    {
      _bottomLeftPoint = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
      _topRightPoint = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
    }
    private void Update()
    {
      _gravityCollider.enabled = !CheckOutOfCamera();
    }

    private bool CheckOutOfCamera()
    {
      var position = transform.position;
      return position.x > _topRightPoint.x
             || position.y > _topRightPoint.y
             || position.x < _bottomLeftPoint.x
             || position.y < _bottomLeftPoint.y;
    }
  }
}