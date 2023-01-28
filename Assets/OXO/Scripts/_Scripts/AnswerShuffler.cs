using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AnswerShuffler : MonoBehaviour
{
    public List<GameObject> answerButtons;


    private void OnEnable()
    {
        foreach (Transform tr in transform)
        {
            answerButtons.Add(tr.gameObject);
        }

        ParentNull();
        AddChildAgain();
    }

    private async void ParentNull()
    {
        await Task.Delay(10);
        foreach (GameObject gO in answerButtons)
        {
            gO.transform.parent = null;
        }
    }

    private async void AddChildAgain()
    {
        await Task.Delay(20);
        answerButtons.ShuffleList();

        for (int i = 0; i < answerButtons.Count; i++)
        {
            answerButtons[i].transform.parent = transform;
        }
    }
}
