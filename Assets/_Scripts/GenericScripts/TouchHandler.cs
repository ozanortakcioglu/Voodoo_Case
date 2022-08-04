using UnityEngine;
using UnityEngine.Events;

public struct TouchInput
{
    public Vector2 FirstScreenPosition;
    public Vector2 ScreenPosition;
    public Vector2 DeltaScreenPosition;
    public int screenWidth;
    public TouchPhase Phase;
}

public delegate void TouchEvent(TouchInput touch);

public class TouchHandler : MonoBehaviour
{

    public bool isActive = true;
    public Camera mainCamera;

    public static TouchEvent onTouchBegan = null, onTouchMoved = null, onTouchEnded = null;
    public UnityAction OnTouch;

    private TouchInput _touch;

    private void Start()
    {
        _touch.screenWidth = Screen.width;
    }

    private void Update()
    {
        if (!isActive) return;

        ApplyTouch();

        switch (_touch.Phase)
        {
            case TouchPhase.NotActive:
                return;
            case TouchPhase.Started:
                onTouchBegan?.Invoke(_touch);
                if (OnTouch != null)
                    OnTouch();
                break;
            case TouchPhase.Moved:
                onTouchMoved?.Invoke(_touch);
                break;
            case TouchPhase.Ended:
                onTouchEnded?.Invoke(_touch);
                break;
        }
    }

    private void ApplyTouch()
    {

        if (!Input.GetMouseButton(0) && !Input.GetMouseButtonUp(0))
        {
            _touch.Phase = TouchPhase.NotActive;
        }
        else
        {
            Vector2 screenPosition = Input.mousePosition;

            if (Input.GetMouseButtonDown(0))
            {
                _touch.Phase = TouchPhase.Started;
                _touch.FirstScreenPosition = screenPosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _touch.Phase = TouchPhase.Ended;
            }
            else
            {
                _touch.Phase = TouchPhase.Moved;
                _touch.DeltaScreenPosition = screenPosition - _touch.ScreenPosition;
            }

            _touch.ScreenPosition = screenPosition;
        }

    }
}