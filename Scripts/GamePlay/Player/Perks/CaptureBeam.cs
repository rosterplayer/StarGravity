using UnityEngine;

namespace StarGravity.GamePlay.Player.Perks
{
  [RequireComponent(typeof(LineRenderer))]
  public class CaptureBeam : MonoBehaviour
  {
    private const float LivingTime = 0.5f;
    
    [SerializeField] private LineRenderer _lineRenderer;
    
    private bool _isReady;
    private float _timeToDisable;
    private Vector3 _endPoint;

    public bool IsActive => _isReady;

    public void Show(Vector3 endPoint)
    {
      _endPoint = endPoint;
      gameObject.SetActive(true);
      Draw();
      _isReady = true;
      _timeToDisable = LivingTime;
    }

    private void Update()
    {
      if (!_isReady)
        return;
      
      Draw();
      _timeToDisable -= Time.deltaTime;
      if (_timeToDisable <= 0) Hide();
    }

    public void Hide()
    {
      _isReady = false;
      gameObject.SetActive(false);
    }

    private void Draw()
    {
      _lineRenderer.SetPosition(0, transform.parent.position);
      _lineRenderer.SetPosition(1, _endPoint);
    }
  }
}