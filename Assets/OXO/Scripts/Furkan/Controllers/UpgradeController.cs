using System;
using Controllers.Money;
using UnityEngine;

namespace Controllers.Upgrade
{
    public class UpgradeController: Singleton<UpgradeController>
    {
        #region Fields

        [Header("Income")]
        [SerializeField] private int incomeIncreaseCostAtStart = 0;
        [SerializeField] private int incomeIncreaseCostUpgrade = 0;
        [SerializeField] private int incomeIncreaseValueAtStart = 0;
        
        [Header("Speed")]
        [SerializeField] private int speedIncreaseCostAtStart = 0;
        [SerializeField] private int speedIncreaseCostUpgrade = 0;
        [SerializeField] private float speedIncreaseValueAtStart = 0;
        
        public static event Action<int> onIncomeUpgrade; 
        public static event Action<float> onSpeedUpgrade;

        #endregion
        
        #region Properties

        public int incomeIncreaseCost
        {
            get => PlayerPrefs.GetInt("incomeIncreaseCost", incomeIncreaseCostAtStart);
            set
            {
                PlayerPrefs.SetInt("incomeIncreaseCost",value);
            }
        }
        
        public int incomeIncreaseValue
        {
            get => PlayerPrefs.GetInt("incomeIncreaseValue", incomeIncreaseValueAtStart);
            set
            {
                PlayerPrefs.SetInt("incomeIncreaseValue",value);
            }
        }
        
        public int speedIncreaseCost
        {
            get => PlayerPrefs.GetInt("speedIncreaseCost", speedIncreaseCostAtStart);
            set
            {
                PlayerPrefs.SetInt("speedIncreaseCost",value);
            }
        }
        
        public float speedIncreaseValue
        {
            get => PlayerPrefs.GetFloat("speedIncreaseValue", speedIncreaseValueAtStart);
            set
            {
                PlayerPrefs.SetFloat("speedIncreaseValue",value);
            }
        }
        
        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            MoneyController.onMoneyChanged += ControlButtonActivity;
        }

        private void OnDisable()
        {
            MoneyController.onMoneyChanged -= ControlButtonActivity;
        }

        private void Start()
        {
            SetUpgradeCostTexts();
        }

        #endregion

        #region Private Methods
        

        private void SetUpgradeCostTexts()
        {
            CanvasManager.instance.SetIncomeUpgradeText(incomeIncreaseCost);
            CanvasManager.instance.SetsSpeedUpgradeText(speedIncreaseCost);
        }

        private void ControlButtonActivity(int totalMoney)
        {
            if (totalMoney < incomeIncreaseCost)
            {
                CanvasManager.instance.ControlIncomeButtonActivity(false);
            }
            else
            {
                CanvasManager.instance.ControlIncomeButtonActivity(true);
            }
            if (totalMoney < speedIncreaseCost)
            {
                CanvasManager.instance.ControlSpeedButtonActivity(false);
            }
            else
            {
                CanvasManager.instance.ControlSpeedButtonActivity(true);
            }
        }
        #endregion

        #region Public Methods

        public void UpgradeIncome()
        {
            if (MoneyController.Instance.SpendMoney(incomeIncreaseCost))
            {
                CanvasManager.instance.AnimateMoneys(CanvasManager.instance.totalMoneyImage.position,CanvasManager.instance.incomeUpgradeIconPosition.position,false,0);
                incomeIncreaseCost += incomeIncreaseCostUpgrade;
                onIncomeUpgrade?.Invoke(incomeIncreaseValue);
                CanvasManager.instance.SetIncomeUpgradeText(incomeIncreaseCost);
                MoneyController.Instance.CheckMoneyForButtons();
            }
        }

        public void UpgradeSpeed()
        {
            if (MoneyController.Instance.SpendMoney(speedIncreaseCost))
            {
                CanvasManager.instance.AnimateMoneys(CanvasManager.instance.totalMoneyImage.position,CanvasManager.instance.speedUpgradeIconPosition.position,false,0);
                speedIncreaseCost += speedIncreaseCostUpgrade;
                onSpeedUpgrade?.Invoke(speedIncreaseValue);
                CanvasManager.instance.SetsSpeedUpgradeText(speedIncreaseCost);
                MoneyController.Instance.CheckMoneyForButtons();
            }
        }
        
        #endregion
    }
  

}