using System;
using UnityEngine;

namespace Wall.Answer
{
    public class TrueAnswerHolder: MonoBehaviour
    {
        #region Fields

        [SerializeField] private Transform wallHolder;
        
        private string _trueAnswer;

        #endregion
        
        #region Properties

        public string trueAnswer => _trueAnswer;
        
        #endregion


        #region Unity Methods

        private void Start()
        {
            _trueAnswer = "";
            _trueAnswer = null;
        }

        private void Update()
        {
            GetTrueAnswer();
        }

        #endregion

        #region Private Methods

        private void GetTrueAnswer()
        {
            if (wallHolder.childCount > 0 && string.IsNullOrEmpty(_trueAnswer))
            {
                WallBangBang createdWall = wallHolder.GetChild(0).GetComponent<WallBangBang>();
                _trueAnswer = createdWall.wallName;
            }
        }

        #endregion
    }
  

}