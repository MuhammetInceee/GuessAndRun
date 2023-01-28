using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Vector3 offset;
    private float zCoord;

    private Camera camera;
    public Camera Camera
    {
        get
        {
            if (camera == null)
            {
                camera = Camera.main;
            }
            return camera;
        }
    }

    private void OnMouseDown()
    {
        zCoord = Camera.WorldToScreenPoint(gameObject.transform.position).z;
        // Offset = gameobject world pos - mouse world pos
        offset = gameObject.transform.position - GetMouseWorldPos();
    }

    private Vector3 GetMouseWorldPos()
    {
        //pixel coordinates (x,y)
        Vector3 mousePoint = Input.mousePosition;

        //z coordinate of game object on screen
        mousePoint.z = zCoord;

        return Camera.ScreenToWorldPoint(mousePoint);
    }
    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + offset;
    }
}
