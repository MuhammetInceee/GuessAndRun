using System;
using Controllers.Upgrade;
using UnityEngine;

namespace Controllers.Money
{
    public class MoneyController: Singleton<MoneyController>
    {
        #region Fields

        [SerializeField] private int startMoney = 0;
        [SerializeField] private int startIncomeValue = 0;
        public static event Action<int> onMoneyChanged;

        private int _collectedMoneyCount = 0;
        #endregion
        
        #region Properties

        public int totalMoney
        {
            get => PlayerPrefs.GetInt("totalMoney", startMoney);
            set
            {
                PlayerPrefs.SetInt("totalMoney", value);
            }
        }

        public int incomeValue
        {
            get => PlayerPrefs.GetInt("incomeValue", startIncomeValue);
            set => PlayerPrefs.SetInt("incomeValue", value);
        }

        public int collectedMoneyCount => _collectedMoneyCount;
        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            UpgradeController.onIncomeUpgrade += IncreaseIncomeValue;
        }

        private void OnDisable()
        {
            UpgradeController.onIncomeUpgrade -= IncreaseIncomeValue;
        }

        private void Start()
        {
            onMoneyChanged?.Invoke(totalMoney);
        }

        #endregion
        
        #region Private Methods
        
        private void IncreaseIncomeValue(int value)
        {
            incomeValue += value;
        }
        #endregion
        
        #region Public Methods

        public void IncreaseTotalMoney(int trueLetterCount)
        {
            _collectedMoneyCount++;
            totalMoney += (int)(incomeValue*trueLetterCount);
            
            onMoneyChanged?.Invoke(totalMoney);
        }

        public bool SpendMoney(int cost)
        {
            if (cost <= totalMoney)
            {
                totalMoney -= cost;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CheckMoneyForButtons()
        {
            onMoneyChanged?.Invoke(totalMoney);
        }
        #endregion
    }
  

}