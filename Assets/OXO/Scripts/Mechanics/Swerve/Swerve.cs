
using UnityEngine;

public class Swerve : MonoBehaviour
{
    public float sensitivity = 0.01f;

    public float clampValue = 2.5f;


    void Update()
    {
        #region MobileInputCheck
        if (Input.GetMouseButtonDown(0)) FingerDown();
        if (Input.GetMouseButton(0)) FingerDrag();
        if (Input.GetMouseButtonUp(0)) FingerUp();
        #endregion
    }

    #region MobileInputFunctions

    Vector2 firstMousePosition;
    Vector2 lastMousePosition;
    Vector2 deltaMousePosition;
    Vector2 movementVector;

    void FingerDown()
    {
        firstMousePosition = Input.mousePosition;
    }

    void FingerDrag()
    {
        lastMousePosition = Input.mousePosition;

        deltaMousePosition = lastMousePosition - firstMousePosition;

        movementVector = deltaMousePosition * sensitivity;

        Vector3 currentPos = transform.position;

        float posX = Mathf.Clamp(currentPos.x + movementVector.x, -clampValue, clampValue); // look here

        transform.position = new Vector3(posX, currentPos.y, currentPos.z);

        firstMousePosition = lastMousePosition;
        
    }

    void FingerUp()
    {
        
    }

    #endregion


}
