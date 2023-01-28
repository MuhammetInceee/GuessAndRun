using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class HelperClass
{
    public static IEnumerator ParticleToggle(this ParticleSystem system, float delay)
    {
        system.Play();
        yield return new WaitForSeconds(delay);
        system.Stop();
    }

    public static IEnumerator AnimatorToggle(this Animator animator, int id, float delay)
    {
        animator.SetBool(id, true);
        yield return new WaitForSeconds(delay);
        animator.SetBool(id, false);
    }
    
    public static void ShuffleList<T>(this IList<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];

            int randomIndex = Random.Range(0, list.Count);

            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public static IEnumerator BoolCloserDelay(this bool boolean, float delay)
    {
        yield return new WaitForSeconds(delay);
        boolean = true;
    }
}


