using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Chronometer : MonoBehaviour
{
    Text selfText;
    public bool waitForGameStart;
    private float elapsedTime;
    private void Start()
    {
        selfText = GetComponent<Text>();
    }
    void Update()
    {
        if (!GameManager.instance.isStarted && waitForGameStart)
        {
            selfText.text = "Waiting";
            elapsedTime = Time.time;
            return;
        }

        selfText.text = (Formater(Time.time - elapsedTime)).ToString();
        if (Input.GetKeyDown(KeyCode.K))
        {
            Time.timeScale = 1;
        }
    }
    public string Formater(float number)
    {
        number = (int)number;

        if (number > 3600)
        {
            return number.ToString("0. Hours.");
        }
        if (number > 60)
        {

            return (number / 60).ToString("0. Min.") + (number - 60).ToString();
        }
        if (number < 60)
        {
            return number.ToString("0. Sec.");
        }

        return number.ToString("#,0");

    }
}
