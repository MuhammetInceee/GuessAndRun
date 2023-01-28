using System;
using JetBrains.Annotations;
using MuhammetInce.DesignPattern.Singleton;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : LazySingleton<UIManager>
{
    [Header("Level Counter:")] 
    [SerializeField] private TextMeshProUGUI levelText;
    
    [Header("Canvases: ")] 
    [SerializeField] private GameObject inGameCanvas;
    [SerializeField] private GameObject leveEndCanvas;
    
    [Header("Level End UI's: ")]
    [SerializeField] private GameObject firstPlace;
    [SerializeField] private GameObject secondPlace;
    [SerializeField] private GameObject thirdPlace;


    private void Start()
    {
        levelText.text = "Lv " + LevelManager.instance.level;
    }

    public void LevelEndVisualizer(string str)
    {
        inGameCanvas.SetActive(false);
        leveEndCanvas.SetActive(true);
        ConfettiManager.instance.Play();
        
        switch (str)
        {
            case "FirstPlace":
                firstPlace.SetActive(true);
                break;
            case "SecondPlace":
                secondPlace.SetActive(true);
                break;
            case "ThirdPlace":
                thirdPlace.SetActive(true);
                break;
            default:
                Debug.LogError("Switch Statement Returning NULL. Please Check Where You Called. :)");
                break;
        }
    }

    public void NextLevelButton()
    {
        LevelManager.instance.IncreaseLevel();
    }
}
