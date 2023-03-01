using System.Collections.Generic;
using System.Threading.Tasks;
using Controllers.Money;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.EventSystems;

public class CanvasManager : Singleton<CanvasManager>
{
    [HideInInspector]
    public static CanvasManager instance;

    [Header("Clue")] 
    [SerializeField] private GameObject clueObj;
    [SerializeField] private Transform clueImage;
    [SerializeField] private TextMeshProUGUI clueText;
    [SerializeField] private Transform clueOpenPos;
    [SerializeField] private Transform clueStartPos;
    
    [Header("Money")] 
    [SerializeField] private Image[] moneyImages;
    [SerializeField] private Transform _totalMoneyImage;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private RectTransform[] moneys;
    
    [Header("Upgrades")] 
    [SerializeField] private CanvasGroup upgradePanel;
    [SerializeField] private TextMeshProUGUI incomeUpgradeCostText;
    [SerializeField] private TextMeshProUGUI speedUpgradeCostText;
    [SerializeField] private Transform _incomeUpgradeIconPosition;
    [SerializeField] private Transform _speedUpgradeIconPosition;

    [SerializeField] private Button _incomeUpgradeButton;
    [SerializeField] private Button _speedUpgradeButton;

   /* [Header("Control Panel")] 
    [SerializeField] private CanvasGroup controlPanel;
    [SerializeField] private Button soundOpenButton;
    [SerializeField] private Button soundCloseButton;
    [SerializeField] private Button vibrationOpenButton;
    [SerializeField] private Button vibrationCloseButton;
    [SerializeField] private Button replayButton;*/
    
    [Header("Others")]
    public GameObject tapToPlayButton;
    public GameObject nextLevelButton;
    public GameObject retryLevelButton;

    public GameObject tutorialRect;
    public GameObject mainMenuRect;
    public GameObject inGameRect;
    public GameObject finishRect;

    public Image levelSliderImage;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI coinText;

    public GameObject winPanel;
    public GameObject failPanel;
    
    private CanvasGroup incomeUpgradeButtonCanvasGroup, speedUpgradeButtonCanvasGroup;
    
    private Tween moneyTween;
    private int moneyIndex = 0;

    private bool _isVibrationEnabled = true;
    public Transform incomeUpgradeIconPosition => _incomeUpgradeIconPosition;
    public Transform speedUpgradeIconPosition => _speedUpgradeIconPosition;
    public Transform totalMoneyImage => _totalMoneyImage;
    public Button incomeUpgradeButton => _incomeUpgradeButton;
    public Button speedUpgradeButton => _speedUpgradeButton;

    private Vector3 moneyImageStartScale;
    public bool isVibrationEnabled => _isVibrationEnabled;
    
    private void OnEnable()
    {
        MoneyController.onMoneyChanged += SetMoneyText;
    }

    private void OnDisable()
    {
        MoneyController.onMoneyChanged -= SetMoneyText;
    }

    private void Awake()
    {
        instance = this;
        mainMenuRect.SetActive(true);
        incomeUpgradeButtonCanvasGroup = _incomeUpgradeButton.GetComponent<CanvasGroup>();
        speedUpgradeButtonCanvasGroup = _speedUpgradeButton.GetComponent<CanvasGroup>();
        moneyImageStartScale = _totalMoneyImage.localScale;
        
        ControlUpgradePanel(true);
    }
    public void TapToPlayButtonClick()
    {
        if (!IsPointerOverUIObject())
        {
            GameManager.instance.StartGame();
        }
       
    }

    public bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition,results);
        return results.Count > 0;
    }
    public void ControlMainPanel(bool val)
    {
        mainMenuRect.SetActive(val);
    }
    public void NextLevel()
    {
        LevelManager.instance.IncreaseLevel();
        LevelManager.instance.RestartScene();
        ES3.Save("list", WallSpawnManager.Instance.tempList);
    }
    public void RestartGame()
    {
        //LevelManager.instance.SetLevel();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
        LevelManager.instance.RestartScene();
        ES3.Save("list", WallSpawnManager.Instance.tempList);
    }



    public void OpenFinishRect(bool isSuccess)
    {

        if (isSuccess)
        {
            winPanel.SetActive(true);
            retryLevelButton.SetActive(false);
            nextLevelButton.SetActive(true);
        }
        else
        {
            failPanel.SetActive(true);
            retryLevelButton.SetActive(true);
            nextLevelButton.SetActive(false);
        }

        inGameRect.SetActive(false);
        finishRect.GetComponent<CanvasGroup>().DOFade(1, 2f);

        finishRect.SetActive(true);
    }

    /*private void ControlPanelsOnLevelChanged()
    {
        mainMenuRect.SetActive(true);
        inGameRect.SetActive(false);
        finishRect.SetActive(false);
        
        winPanel.SetActive(false);
        failPanel.SetActive(false);
        
        retryLevelButton.SetActive(false);
        nextLevelButton.SetActive(false);
    }*/
    public void AnimateMoneys(Vector3 startPosition,Vector3 endPosition,int totalMoney)
    {
        Transform money = moneyImages[moneyIndex].transform;
        
        money.gameObject.SetActive(true);
        money.position = startPosition;
        money.DOJump(endPosition, 2, 1, 0.5f).OnComplete(()=>DisableMoneyAndIncreaseTotalMoney(money.gameObject,totalMoney));

        if (moneyIndex < moneyImages.Length-1)
        {
            moneyIndex++;
        }
        else
        {
            moneyIndex = 0;
        }
    }

    public void SetMoneyText(int totalMoney)
    {
        moneyText.text = totalMoney.ToString();
    }
    private void DisableMoneyAndIncreaseTotalMoney(GameObject money,int totalMoney)
    {
      /*  SetMoneyText(totalMoney);
        money.SetActive(false);
        
        if (!moneyTween.IsPlaying())
        {
            moneyTween=totalMoneyImage.DOPunchScale(Vector3.one * 0.5f, 0.25f);
        }*/
      
    }

    #region Upgrades

    public void ControlUpgradePanel(bool value)
    {
        if (value)
        {
            upgradePanel.gameObject.SetActive(true);
            upgradePanel.DOFade(1, 0.25f);
        }
        else
        {
            upgradePanel.DOFade(0, 0.25f).OnComplete((() => upgradePanel.gameObject.SetActive(false)));
        }
    }
    public void SetIncomeUpgradeText(int value)
    {
        incomeUpgradeCostText.text = value.ToString();
    }

    public void SetsSpeedUpgradeText(int value)
    {
        speedUpgradeCostText.text = value.ToString();
    }

    public void ControlIncomeButtonActivity(bool value)
    {
        incomeUpgradeButton.interactable = value;
        if (!value)
        {
            incomeUpgradeButtonCanvasGroup.alpha = 0.5f;
        }
        else
        {
            incomeUpgradeButtonCanvasGroup.alpha = 1;
        }
    }
    public void ControlSpeedButtonActivity(bool value)
    {
        speedUpgradeButton.interactable = value;
        if (!value)
        {
            speedUpgradeButtonCanvasGroup.alpha = 0.5f;
        }
        else
        {
            speedUpgradeButtonCanvasGroup.alpha = 1;
        }
    }

    public void ShakeMoneyImage()
    {
        if (moneyTween != null)
        {
            if (!moneyTween.IsPlaying())
            {
                moneyTween=_totalMoneyImage.DOPunchScale(Vector3.one * 1.15f, 0.2f);
            }
        }
        else
        {
            moneyTween=_totalMoneyImage.DOPunchScale(Vector3.one * 1.15f, 0.2f);
        }
    }

    public void ControlVibration()
    {
        if (_isVibrationEnabled)
        {
            _isVibrationEnabled = false;
        }
        else
        {
            _isVibrationEnabled = true;
        }
    }
    
    public async void AnimateMoneys(Vector2 startPosition,Vector2 endPosition,bool isEarned,int trueLetterCount)
    {
        moneyTween.Kill();
        _totalMoneyImage.transform.localScale=moneyImageStartScale;
        
        for (int i = 0; i < 5; i++)
        {
            RectTransform money = moneys[moneyIndex];
            money.gameObject.SetActive(true);
            money.position = startPosition;
            
            if (isEarned)
            {
                if (i < 4)
                {
                    money.DOJump(endPosition, 2, 1, 0.5f).OnComplete((() => money.gameObject.SetActive(false)));
                }
                else if(i == 4)
                {
                    money.DOJump(endPosition, 2, 1, 0.5f).OnComplete((() => IncreaseMoney(trueLetterCount)));
                }
            }
            else
            {
                money.DOJump(endPosition, 2, 1, 0.5f).OnComplete((() => money.gameObject.SetActive(false)));
            }

            if (moneyIndex < moneys.Length-1)
            {
                moneyIndex++;
            }
            else
            {
                moneyIndex = 0;
            }

            await Task.Delay(50);
        }
        
    }

    private void IncreaseMoney(int trueLetterCount)
    {
        MoneyController.Instance.IncreaseTotalMoney(trueLetterCount);
        moneyTween=_totalMoneyImage.DOPunchScale(Vector3.one * 0.5f, 0.25f);

        foreach (var mon in moneys)
        {
            mon.gameObject.SetActive(false);
        }
    }

    public void ControlClue(bool val,string clue)
    {
        if (val)
        {
            clueObj.SetActive(true);
            clueImage.position = clueStartPos.position;
            clueText.text = "";
            clueText.text = clue;
            clueImage.DOMove(clueOpenPos.position, 0.5f);
        }
        else
        {
            clueImage.DOMove(clueStartPos.position, 0.5f).OnComplete((() => clueObj.SetActive(false))); 
        }
    }

    public void ForceCloseClue()
    {
        clueImage.position = clueStartPos.position;
        clueObj.SetActive(false);
    }
/*
    public void ControlPanelController(int index)
    {
        if (index == 1)
        {
            isControlPanelOpened = true;
            controlPanel.gameObject.SetActive(true);
            controlPanel.DOFade(1, 0.5f);
        }
        else
        {
            controlPanel.DOFade(0, 0.5f).OnComplete(DisablePanel);
        }
    }

    private void DisablePanel()
    {
        controlPanel.alpha = 0;
        controlPanel.gameObject.SetActive(false);
        isControlPanelOpened = false;
    }

    public void ControlSoundButtons(int buttonIndex)
    {
        if (buttonIndex == 1)
        {
            soundOpenButton.gameObject.SetActive(false);
            soundCloseButton.gameObject.SetActive(true);
            SoundController.ControlSound();
        }
        else
        {
            soundCloseButton.gameObject.SetActive(false);
            soundOpenButton.gameObject.SetActive(true);
            SoundController.ControlSound();
        }
    }
    public void ControlVibrationButtons(int buttonIndex)
    {
        if (buttonIndex == 1)
        {
            vibrationOpenButton.gameObject.SetActive(false);
            vibrationCloseButton.gameObject.SetActive(true);
            VibrationController.ControlVibration();
        }
        else
        {
            vibrationCloseButton.gameObject.SetActive(false);
            vibrationOpenButton.gameObject.SetActive(true);
            VibrationController.ControlVibration();
        }
    }
    public void ControlReplayButton()
    {
        DisablePanel();
        RestartGame();
        GameController.Instance.UpdateState(GameStates.Start);
        mainMenuRect.SetActive(true);
    }
    */
    #endregion
}