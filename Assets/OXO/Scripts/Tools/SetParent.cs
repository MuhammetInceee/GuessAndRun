using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SetParent : MonoBehaviour
{
    public bool active = true;
    [Space(30)]
    public List<ObjectsClass> objectsClassList;
    public float delayForStart = 0;

    [Serializable]
    public class ObjectsClass
    {
        public Transform objectToBeChild;
        public Transform parent;
        public float delay = 0;
    }

    private void Awake()
    {
        if (active)
        {
            StartCoroutine(SetParentObjects());
        }
    }
    public IEnumerator SetParentObjects()
    {
        yield return new WaitForSeconds(delayForStart);
        for (int i = 0; i < objectsClassList.Count; i++)
        {
            yield return new WaitForSeconds(objectsClassList[i].delay);
            objectsClassList[i].objectToBeChild.SetParent(objectsClassList[i].parent);
        }
    }
}
