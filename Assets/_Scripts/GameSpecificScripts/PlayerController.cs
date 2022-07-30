using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float leftLimit = -2f;
    public float rightLimit = 2f;
    public float forwardSpeed = 4f;
    public float leftRightSpeed = 2;

    private float swipeSensivity;
    private float maximumSensivity = 200f;
    private Vector3 targetPos;
    private bool controlsEnabled = true;

    private void Start()
    {
        TouchHandler.onTouchBegan += TouchBegan;
        TouchHandler.onTouchMoved += TouchMoved;
        TouchHandler.onTouchEnded += TouchEnded;

    }

    private void Update()
    {
        if(controlsEnabled)
            Movement();
    }

    private void TouchBegan(TouchInput touch)
    {
        targetPos = transform.position;
    }

    private void TouchEnded(TouchInput touch)
    {
        targetPos = transform.position;
        swipeSensivity = 0f;
    }

    private void TouchMoved(TouchInput touch)
    {
        swipeSensivity = Mathf.Abs(touch.DeltaScreenPosition.x) * leftRightSpeed;

        if (touch.DeltaScreenPosition.x < 0)
            targetPos = new Vector3(transform.position.x - swipeSensivity / 200f, transform.position.y, transform.position.z);
        if (touch.DeltaScreenPosition.x > 0)
            targetPos = new Vector3(transform.position.x + swipeSensivity / 200f, transform.position.y, transform.position.z);
    }

    private void Movement()
    {     
        targetPos = new Vector3(Mathf.Clamp(targetPos.x, leftLimit, rightLimit), targetPos.y, targetPos.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.unscaledDeltaTime * swipeSensivity / 2f);
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1f), Time.unscaledDeltaTime * forwardSpeed);
    }

    private void OnDestroy()
    {
        TouchHandler.onTouchBegan -= TouchBegan;
        TouchHandler.onTouchMoved -= TouchMoved;
        TouchHandler.onTouchEnded -= TouchEnded;
    }
}
