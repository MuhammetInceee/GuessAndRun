using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Money : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(OpenAnimation());
    }
    public IEnumerator OpenAnimation()
    {
        float yOffset = 0.5f;
        float duration = 0.5f;

        transform.position += Vector3.down * yOffset;
        transform.localEulerAngles = Vector3.up * Random.value * 100;

        Vector3 tempLocalScale = transform.localScale;
        transform.localScale = Vector3.zero;


        transform.DOScale(tempLocalScale, duration * 2);
        transform.DOLocalRotate(Vector3.up * Random.value * 10, duration * 2);
        transform.DOMoveY(transform.position.y + yOffset, duration);

        yield return null;
    }

}
