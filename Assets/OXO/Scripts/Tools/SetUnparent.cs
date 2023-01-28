using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SetUnparent : MonoBehaviour
{
    public List<Transform> willUnparentObjects;
    [Header("Settings")]
    public float firstWaitDelay;
    public float unparentBetweenDelay;
    public bool setPos;
    public Vector3 setPosVector;


    private void Awake()
    {
        StartCoroutine(SetUnparentObjects());
    }
    IEnumerator SetUnparentObjects()
    {
        yield return new WaitForSeconds(firstWaitDelay);
        for (int i = 0; i < willUnparentObjects.Count; i++)
        {
            willUnparentObjects[i].parent = null;
            if (setPos)
            {
                willUnparentObjects[i].transform.position = Vector3.zero;
            }
            yield return new WaitForSeconds(firstWaitDelay);
        }
        yield return null;
    }
}
