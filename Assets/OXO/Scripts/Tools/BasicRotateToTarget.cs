using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BasicRotateToTarget : MonoBehaviour
{
    [Header("Target Object Transform")]
    public Transform target;

    public bool autoTarget;
    public bool x, y, z;

    protected Vector3 offset;

    private void Start()
    {
        if (autoTarget && !target)
        {
            //target = FindObjectOfType<PlayerController>()?.transform;
        }
        offset = transform.eulerAngles - target.eulerAngles;
    }
    private void Update()
    {
        if (autoTarget && !target)
        {
            //target = FindObjectOfType<PlayerController>().transform;
        }
        Vector3 vector = new Vector3(target.eulerAngles.x * BoolConverter(x), target.eulerAngles.y * BoolConverter(y), target.eulerAngles.z * BoolConverter(z));
        transform.rotation = Quaternion.Euler(offset + vector);

    }
    public int BoolConverter(bool boolen)
    {
        if (boolen)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}
