using System.Collections.Generic;
using Character.Crown;
using UnityEngine;

namespace Crown
{
    public class CrownController: Singleton<CrownController>
    {
        #region Fields

        [SerializeField] private CharacterCrownController[] players;
        [SerializeField] private Transform endLine;

        public List<CharacterCrownController> sortedList;

        private bool _isCheckEnabled = true;
        
        #endregion
        
        #region Properties

        public bool isCheckEnabled
        {
            get => _isCheckEnabled;
            set
            {
                _isCheckEnabled = value;
            }
        }
        
        #endregion

        #region Unity Methods

        void Start()
        {
            foreach (var player in players)
            {
                sortedList.Add(player);
            }
        }
        
        void Update()
        {
            if (_isCheckEnabled)
            {
                CheckForFirstContestant(); 
            }
        }

        #endregion

        #region Private Methods

        private void CheckForFirstContestant()
        {
            int min;
            CharacterCrownController temp;

            for (int i = 0; i < sortedList.Count; i++)
            {
                min = i;
                for (int j = i+1; j < sortedList.Count; j++)
                {
                    if (Vector3.Distance(sortedList[j].transform.position, endLine.transform.position) <
                        Vector3.Distance(sortedList[min].transform.position, endLine.transform.position))
                    {
                        min = j;
                    }
                }

                if (min != i)
                {
                    temp = sortedList[i];
                    sortedList[i] = sortedList[min];
                    sortedList[min] = temp;

                    foreach (var player in sortedList)
                    {
                        player.ControlCrown(false);
                    }
                    sortedList[0].ControlCrown(true);
                }
            }
        }
        
        #endregion

        #region Public Methods

        

        #endregion
    }
  

}