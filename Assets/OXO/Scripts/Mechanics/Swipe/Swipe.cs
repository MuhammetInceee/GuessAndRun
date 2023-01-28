using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour
{
    public float swipeRange;
    Vector2 firstMousePosition;
    Vector2 currentMousePosition;
    public bool stopTouch;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) FingerDown();
        if (Input.GetMouseButton(0)) FingerDrag();
        if (Input.GetMouseButtonUp(0)) FingerUp();

    }
    public void FingerDown()
    {
        firstMousePosition = Input.mousePosition;
    }
    public void FingerDrag()
    {
        currentMousePosition = Input.mousePosition;
        Vector2 distance = currentMousePosition - firstMousePosition;
        if (!stopTouch)
        {
            if (distance.x < -swipeRange)
            {
                SwipeLeft();
                stopTouch = true;
            }
            if (distance.x > swipeRange)
            {
                SwipeRight();
                stopTouch = true;
            }
            if (distance.y > swipeRange)
            {
                SwipeUp();
                stopTouch = true;
            }
            if (distance.y < -swipeRange)
            {
                SwipeDown();
                stopTouch = true;
            }
        }

    }
    public void FingerUp()
    {
        stopTouch = false;
    }
    public void SwipeUp()
    {
        Debug.Log("Swipe UP");
    }
    public void SwipeDown()
    {
        Debug.Log("Swipe Down");
    }
    public void SwipeRight()
    {
        Debug.Log("Swipe Right");
    }
    public void SwipeLeft()
    {
        Debug.Log("Swipe Left");
    }
}
