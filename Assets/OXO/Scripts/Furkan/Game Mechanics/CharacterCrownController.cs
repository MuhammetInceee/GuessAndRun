using UnityEngine;

namespace Character.Crown
{
    public class CharacterCrownController: MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObject crown;

        #endregion

        #region Public Methods

        public void ControlCrown(bool val)
        {
            crown.SetActive(val);
        }

        #endregion
    }
  

}