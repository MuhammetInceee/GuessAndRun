using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Caller : MonoBehaviour
{
    public Image Image;
    public Button Button;
    public GameObject AnswerImage;

    public List<Button> buttonList;
    public Button otherButton1;
    public Button otherButton2;

    private void Awake()
    {
        Image = GetComponent<Image>();
        Button = GetComponent<Button>();
        AnswerImage = transform.GetChild(1).gameObject;
    }

    public void Callers(bool isTrue)
    {
        //AnswerManager.Instance.Answer(isTrue,this);
        InteractableCloser();
    }

    private void InteractableCloser()
    {
        foreach (Transform tr in transform.parent)
        {
            tr.GetComponent<Button>().interactable = false;
        }
    }
}