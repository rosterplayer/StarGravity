using UnityEngine;
using UnityEngine.EventSystems;

namespace StarGravity.Infrastructure.Services.Input
{
  public class InputService : MonoBehaviour, IInputService
  {
    private const int Epsilon = 20;
    
    private bool _clicked;
    private bool _upClick;
    private bool _downClick;
    private bool _blockExternalInput;
    private Vector3 _startMousePosition;

    public bool PressInput => !_blockExternalInput && _clicked;
    public bool UpInput => !_blockExternalInput && _upClick;
    public bool DownInput => !_blockExternalInput && _downClick;
    public bool Paused => _blockExternalInput;

    private void Update()
    {
      // if (UnityEngine.Input.touchCount > 0)
      //   OnMobileTouch();
      // else
        //OnMouseClick();
    }

    public void Run()
    {
      OnMouseClick();
    }

    public void GainControl() => _blockExternalInput = false;

    public void ReleaseControl() => _blockExternalInput = true;

    private void OnMouseClick()
    {
      if (EventSystem.current.IsPointerOverGameObject())
        return;

      if (UnityEngine.Input.GetMouseButtonDown(0))
      {
        _clicked = true;
        _startMousePosition = UnityEngine.Input.mousePosition;
      }
      else
        _clicked = false;

      if (UnityEngine.Input.GetKeyDown(KeyCode.UpArrow) || UnityEngine.Input.GetKeyDown(KeyCode.W) || SwipedUpByMouse())
        _upClick = true;
      else
        _upClick = false;
      
      if (UnityEngine.Input.GetKeyDown(KeyCode.DownArrow) || UnityEngine.Input.GetKeyDown(KeyCode.S) || SwipedDownByMouse())
        _downClick = true;
      else
        _downClick = false;
    }

    private void OnMobileTouch()
    {
      if (EventSystem.current.IsPointerOverGameObject())
        return;

      Touch touch = UnityEngine.Input.GetTouch(0);
      _clicked = touch.phase == TouchPhase.Began;
      
      if (touch.phase == TouchPhase.Moved && touch.deltaPosition.y < -Epsilon)
        _upClick = true;
      else
        _upClick = false;
      
      if (touch.phase == TouchPhase.Moved && touch.deltaPosition.y > Epsilon)
        _downClick = true;
      else
        _downClick = false;
    }

    private bool SwipedUpByMouse() => 
      UnityEngine.Input.GetMouseButton(0) && UnityEngine.Input.mousePosition.y > _startMousePosition.y + Epsilon;

    private bool SwipedDownByMouse() => 
      UnityEngine.Input.GetMouseButton(0) && UnityEngine.Input.mousePosition.y < _startMousePosition.y - Epsilon;
  }
}