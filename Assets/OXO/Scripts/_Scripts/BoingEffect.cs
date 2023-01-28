using DG.Tweening;
using UnityEngine;

public class BoingEffect : MonoBehaviour
{
    private void OnEnable()
    {
        transform.localScale = Vector3.zero;

        transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.InOutBack);
    }
}
