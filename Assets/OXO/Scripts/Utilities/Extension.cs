using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
//using Redcode.Moroutines;
using System.ComponentModel;
using Component = System.ComponentModel.Component;
using Random = UnityEngine.Random;
using System.Linq;

public static class Extension
{
    public static float Mean(this Vector3 vector)
    {
        if (vector.z == 0)
            return (vector.x + vector.y) / 2f;
        else
            return (vector.x + vector.y + vector.z) / 3f;
    }

    public static Vector3 Vec2ToVec3(this Vector3 vector)
    {
        Vector3 newVector = new Vector3(vector.x, 0, vector.y);
        return newVector;
    }

    public static Vector3 DivideByVector(this Vector3 vector1, Vector3 vector2)
    {
        Vector3 newVector = vector1;
        if (vector2.x != 0)
            newVector.x /= vector2.x;
        if (vector2.y != 0)
            newVector.y /= vector2.y;
        if (vector2.z != 0)
            newVector.z /= vector2.z;
        return newVector;
    }
    public static Transform SetParent(this Transform thisTransform, Transform parent, float duration)
    {
        //Moroutine.Create(SetParentWithDelay(thisTransform, parent, duration)).Run();

        return parent;
    }
    public static float Clamp(this float current, float min, float max)
    {
        float number = Mathf.Clamp(current, min, max);

        return number;
    }
    public static int Clamp(this int current, int min, int max)
    {
        int number = Mathf.Clamp(current, min, max);
        current = number;
        return current;
    }
    public static IEnumerator SetParentWithDelay(Transform changeableTransform, Transform parent, float duration)
    {
        yield return new WaitForSeconds(duration);
        changeableTransform.SetParent(parent);
    }
    public static IEnumerator LerpToValue(this int value, int endValue, float seconds, Action<float> action, MonoBehaviour mono = null)
    {
        float startTime = Time.time;
        float valueToLerp = value;

        while (Time.time - startTime < seconds)
        {
            valueToLerp = Mathf.Lerp(value, endValue, (Time.time - startTime) / (seconds));
            action?.Invoke(valueToLerp);
            yield return new WaitForEndOfFrame();
        }
        if (mono)
        {
            DOTween.Kill(mono);
            mono.transform.DOShakePosition(.1f, 10f);
        }
        action?.Invoke(endValue);

    }
    public static void CallWithDelay(this MonoBehaviour mono, Action method, float delay)
    {
        mono.StartCoroutine(CallWithDelayIE(method, delay));
    }
    public static void Test(this MonoBehaviour mono, Action method, float delay)
    {
        mono.StartCoroutine(CallWithDelayIE(method, delay));
    }
    private static IEnumerator CallWithDelayIE(Action method, float delay)
    {
        yield return new WaitForSeconds(delay);
        method();
    }
    public static T GetRandomElementEnd<T>(this IEnumerable<T> enumerable,int endIndex)
    {

        return enumerable.ElementAt(Random.Range(0, endIndex));
    }
    public static void FieldOfView(this Cinemachine.CinemachineVirtualCamera cm,float endValue, float delay)
    {
        cm.StartCoroutine(IEFieldOfView(cm,endValue, delay));
    }
    private static IEnumerator IEFieldOfView(Cinemachine.CinemachineVirtualCamera cm, float endValue, float delay)
    {
        float timeElapsed = 0;
        float valueToLerp;
        float startValue = cm.m_Lens.FieldOfView;
        while (timeElapsed < delay)
        {
            valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / delay);
            timeElapsed += Time.deltaTime;
            cm.m_Lens.FieldOfView = valueToLerp;
            yield return null;
        }
        valueToLerp = endValue;
    }

}

