using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Key
{
    public class KeyboardController : Singleton<KeyboardController>
    {
        #region Fields

        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private TextMeshProUGUI placeHolderText,lastInputText;
        [SerializeField] private float clueOpenDelay = 0;
        [SerializeField] private float clueCloseDelay = 0;
        [SerializeField] private TutorialSettings tutorial;
        
        private List<int> openedLetters = new List<int>();
        public List<int> trueLetters = new List<int>();

        private TouchScreenKeyboard keyboard;
        private System.Diagnostics.Process p;

        private int placeHolderIndex = 0;
        private int enteredLetterCount = 0;
        private int randomLetter=0;
        
        private string keyboardText;
        private string trueAnswerText;
        private string placeHolderValue;
        
        private bool isOpened = false;
        private bool isCloseEnabled = false;

        #endregion

        #region Properties

        #endregion

        #region Unity Methods

        private void Update()
        {
            CheckForText();
        }

        #endregion

        #region Private Methods

        private void GetText()
        {
            // BasicDebugger.BasicDebugger.Instance.AddOnScreenDebugMessage("GetText",5,Color.white,Color.black);
            keyboardText = inputField.text.ToLower();
            keyboardText = keyboardText.Replace(" ", string.Empty);

            GetTrueAnswer(false, trueAnswerText);
        }

        private void CheckForText()
        {
            if (Application.isEditor)
            {
                if (p != null)
                {
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        GetText();
                        CompareTexts();
                        
                        if (PlayerPrefs.GetInt("tutorial1") == 0)
                        {
                            tutorial.LineCloser();
                            PlayerPrefs.SetInt("tutorial1", 1);
                        }
                    }
                }
            }
            else
            {
                if (keyboard != null && keyboard.status == TouchScreenKeyboard.Status.Done)
                {
                    keyboard = null;
                    GetText();
                    CompareTexts();
                    
                    if (PlayerPrefs.GetInt("tutorial1") == 0)
                    {
                        tutorial.LineCloser();
                        PlayerPrefs.SetInt("tutorial1", 1);
                    }
                    
                    Debug.Log($"<color=orange><b>Pressed Return</b></color>");
                }
                // if (!TouchScreenKeyboard.visible && keyboard != null)
                // {
                //     if (Input.GetKeyDown(KeyCode.Return))
                //     {
                //         GetText();
                //         CompareTexts();
                //     }
                // } 
            }
        }

        #endregion

        #region Public Methods

        public void GetTrueAnswer(bool value, string trueAnswer)
        {
            if (!isOpened)
            {
                isOpened = true;

                if (value)
                {
                    inputField.gameObject.SetActive(true);
                    randomLetter = Random.Range(0, trueAnswer.Length);
                    
                    openedLetters.Add(randomLetter);
                    placeHolderValue = "";
                    
                    for (int i = 0; i < trueAnswer.Length; i++)
                    {
                        if (randomLetter == i)
                        {
                            placeHolderValue += trueAnswer[i];
                        }
                        else
                        {
                            if (trueAnswer[i].ToString() == " ")
                            {
                                placeHolderValue += " ";
                            }
                            else
                            {
                                placeHolderValue += "_";
                            }
                        }
                    }

                    placeHolderText.text ="<mspace=mspace=10>"+placeHolderValue+"</mspace>";
                    trueAnswerText = trueAnswer;
                }
            }

            if (!value)
            {
                openedLetters.Clear();
                CanvasManager.instance.ForceCloseClue();
                isOpened = false;
            }
        }

        public void OnAnswerCanShow()
        {
            StartCoroutine(SelectInputField()); // Only work coroutine

            OpenCloseKeyboard(true);
            StartCoroutine(OpenClue());
        }

        IEnumerator OpenClue()
        {
            yield return new WaitForSeconds(clueOpenDelay);

            while (true)
            {
                randomLetter = Random.Range(0, trueAnswerText.Length);

                if (openedLetters.Count == trueAnswerText.Length)
                {
                    break;
                }
                else
                {
                    if (!openedLetters.Contains(randomLetter))
                    {
                        openedLetters.Add(randomLetter);
                        break;
                    }
                }
            }

            string clueText = "";
            for (int i = 0; i < trueAnswerText.Length; i++)
            {
                if (!openedLetters.Contains(i))
                {
                    clueText += "_";
                }
                else
                {
                    clueText += trueAnswerText[i];
                }
            }

            if (isOpened)
            {
                CanvasManager.instance.ControlClue(true,clueText);
                StartCoroutine(CloseClue());
            }
        }
        
        IEnumerator CloseClue()
        {
            yield return new WaitForSeconds(clueCloseDelay);

            if (isOpened)
            {
                CanvasManager.instance.ControlClue(false,trueAnswerText);
                StartCoroutine(OpenClue());
            }
            
        }
        IEnumerator SelectInputField() // Only work coroutine
        {
            yield return new WaitForEndOfFrame();
            inputField.ActivateInputField();
        }

        public void CompareTexts()
        {
            int trueLetterCount = 0;
            int falseLetterCount = 0;
            float truePercentage = 0;
            trueAnswerText = trueAnswerText.Replace(" ", String.Empty);

            if (keyboardText.Length >= trueAnswerText.Length)
            {
                for (int i = 0; i < trueAnswerText.Length; i++)
                {
                    if (trueAnswerText[i].Equals(keyboardText[i]))
                    {
                        trueLetterCount++;
                        trueLetters.Add(i);
                    }
                    else
                    {
                        falseLetterCount++;
                    }

                    if (keyboardText.Length > trueAnswerText.Length)
                    {
                        falseLetterCount += keyboardText.Length - trueAnswerText.Length;
                    }
                }
            }
            else
            {
                for (int i = 0; i < keyboardText.Length; i++)
                {
                    if (trueAnswerText[i].Equals(keyboardText[i]))
                    {
                        trueLetterCount++;
                        trueLetters.Add(i);
                    }
                    else
                    {
                        falseLetterCount++;
                    }
                }

                falseLetterCount += trueAnswerText.Length - keyboardText.Length;
            }

            if (trueLetterCount > 0)
            {
                truePercentage = ((float)trueLetterCount / (trueLetterCount + falseLetterCount));
            }
            else
            {
                truePercentage = 0;
            }
            AnswerManager.Instance.Answer(truePercentage * 100, trueLetterCount);
            StopAllCoroutines();
        }

        public void OpenCloseKeyboard(bool value)
        {
            if (value)
            {
                if (Application.isEditor)
                {
                    p = System.Diagnostics.Process.Start("osk.exe");
                }
                else
                {
                    keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
                }
            }
            else
            {
                if (Application.isEditor)
                {
                    p = null;
                }
                else
                {
                    keyboard.active = false;
                }
            }
        }

     public void ChangePlaceHolderText()
        {
            if (!string.IsNullOrEmpty(placeHolderValue))
            {
                string placeHolderVal = placeHolderValue;
                string changedVal = "";
                
                if (inputField.text.Length > enteredLetterCount)
                {
                    if (PlayerPrefs.GetInt("tutorial2") == 0)
                    {
                        tutorial.AnswerCloser();
                        PlayerPrefs.SetInt("tutorial2",1);
                    }
                    
                    for (int i = 0; i < placeHolderVal.Length; i++)
                    {
                        if (placeHolderIndex >= i && placeHolderVal[i].ToString() != " ")
                        {
                            changedVal += " ";
                        }
                        else
                        {
                            changedVal += placeHolderVal[i];
                        }
                    }

                    placeHolderValue = changedVal;
                    if (placeHolderIndex < placeHolderValue.Length-1)
                    {
                        placeHolderIndex += 1;
                    }
                    
                    placeHolderText.text ="<mspace=mspace=10>"+placeHolderValue+"</mspace>";
                }
                else
                {
                    if (enteredLetterCount > 0 && enteredLetterCount-1<placeHolderValue.Length)
                    { 
                        string newChangedVal = "";
                        changedVal = placeHolderValue;

                       for (int i = 0; i < changedVal.Length; i++)
                       {
                           if (i != enteredLetterCount - 1)
                           {
                               newChangedVal += changedVal[i];
                           }
                           else
                           {
                               if (enteredLetterCount - 1 == randomLetter)
                               {
                                   newChangedVal += trueAnswerText[randomLetter];
                               }
                               else
                               {
                                   newChangedVal += "_";
                               }
                           }
                       }
                       changedVal = newChangedVal;
                       placeHolderValue = changedVal;
                    
                       if (placeHolderIndex > 0)
                       {
                           placeHolderIndex -= 1;
                       }
                    
                       placeHolderText.text ="<mspace=mspace=10>"+placeHolderValue+"</mspace>";
                   }
                }

                enteredLetterCount = inputField.text.Length;
                if (PlayerPrefs.GetInt("tutorial1") == 0)
                {
                    if (enteredLetterCount == trueAnswerText.Length)
                    {
                        tutorial.LineEnter();
                    }
                }
                
            }
        }

        public async void SetInputFieldTextColor(bool val)
        {
            string inputFieldLastText = inputField.text;
            if (val)
            {
                inputField.text="<color=green>"+ inputFieldLastText +"</color>";
            }
            else
            {
                string coloredInputFieldLastText = "";
                for (int i = 0; i < inputField.text.Length; i++)
                {
                    if (trueLetters.Contains(i))
                    {
                        coloredInputFieldLastText +="<color=green>"+ inputFieldLastText[i] +"</color>";
                    }
                    else
                    {
                        coloredInputFieldLastText +="<color=red>"+ inputFieldLastText[i] +"</color>";
                    }
                }
                lastInputText.gameObject.SetActive(true);
                lastInputText.text = coloredInputFieldLastText;
                inputField.text = "";
                placeHolderText.gameObject.SetActive(false);
            }

            await Task.Delay(2000);
            
            placeHolderText.gameObject.SetActive(true);
            lastInputText.text = "";
            lastInputText.gameObject.SetActive(false);
            keyboardText = "";
            placeHolderText.text = "";
            inputField.text = "";
            
            trueLetters.Clear();
            placeHolderIndex = 0;
            enteredLetterCount = 0;
            
            inputField.gameObject.SetActive(false);
        }
        #endregion
    }
}