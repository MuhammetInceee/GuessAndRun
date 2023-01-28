using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CollisionDebuger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        Debug.Log($"[{gameObject.name}]<color=#5fe769><b>OnTriggerEnter: {other.gameObject.name}</b> </color>");
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"<color=cyan><b>OnCollisionEnter: {collision.gameObject.name}</b> </color>");
    }
}
