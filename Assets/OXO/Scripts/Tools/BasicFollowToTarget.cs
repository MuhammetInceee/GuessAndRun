using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BasicFollowToTarget : MonoBehaviour
{
    [Header("Target Object Transform")]
    public Transform target;

    public bool autoTarget;
    public bool x = true, y = true, z = true;

    protected Vector3 offset;
    public Vector3 followOffset;

    public float lerpValue = 1;
    public int updateMethod;

    private IEnumerator Start()
    {
        yield return new WaitForFixedUpdate();

        if (autoTarget && !target)
        {
            //target = FindObjectOfType<PlayerController>()?.transform;
            transform.localPosition = Vector3.zero;
        }
        if (target)
        {
            offset = transform.position - target.position;
        }
    }
    private void FixedUpdate()
    {
        if (updateMethod == 0)
        {
            if (autoTarget && !target)
            {
                //target = FindObjectOfType<PlayerController>().transform;
            }
            Vector3 vector = new Vector3(target.position.x * BoolConverter(x), target.position.y * BoolConverter(y), target.position.z * BoolConverter(z));
            vector += offset;

            transform.position = Vector3.Lerp(transform.position, vector + followOffset, lerpValue);
        }
    }
    private void LateUpdate()
    {
        if (updateMethod == 1)
        {
            if (autoTarget && !target)
            {
                //target = FindObjectOfType<PlayerController>().transform;
            }
            Vector3 vector = new Vector3(target.position.x * BoolConverter(x), target.position.y * BoolConverter(y), target.position.z * BoolConverter(z));
            vector += offset;

            transform.position = Vector3.Lerp(transform.position, vector, lerpValue);
        }
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
