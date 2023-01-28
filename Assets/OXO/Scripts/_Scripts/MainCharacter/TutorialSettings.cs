using UnityEngine;

public class TutorialSettings : MonoBehaviour
{
    [Header("Line Tutorial")] 
    public GameObject lineHand;
    public GameObject lineText;
    [Header("Answer Tutorial")] 
    public GameObject answerHand;
    public GameObject answerText;
    
    public void LineEnter()
    {
        if (PlayerPrefs.GetInt("tutorial1") == 0)
        {
            lineHand.SetActive(true);
            lineText.SetActive(true);
        }
    }

    public void AnswerEnter()
    {
        if (PlayerPrefs.GetInt("tutorial2") == 0)
        {
            answerHand.SetActive(true);
            answerText.SetActive(true);
        }
    }

    public void LineCloser()
    {
        lineHand.SetActive(false);
        lineText.SetActive(false);
    }
    public void AnswerCloser()
    {
        answerHand.SetActive(false);
        answerText.SetActive(false);
    }
}