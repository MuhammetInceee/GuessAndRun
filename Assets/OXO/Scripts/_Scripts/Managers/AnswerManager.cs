using System.Collections;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Controllers.Money;
using DG.Tweening;
using Key;
using MuhammetInce.DesignPattern.Singleton;
using UnityEngine;
using UnityEngine.UI;

public class AnswerManager : LazySingleton<AnswerManager>
{
    private MainCharacter MainCharacter => MainCharacter.Instance;
    private ManagerGame GameManager => ManagerGame.Instance;

    [SerializeField] private float truePercentage = 0;
    [SerializeField] private float _sakilDelay = 0;

    private float percentageValue = 0;

    public async void Answer(float percentage, int letterCount)
    {
        percentageValue = percentage;
        if (percentage >= truePercentage)
        {
            // caller.AnswerImage.SetActive(true);
            MainCharacter.runSpeed *= GameManager.PlayerRunMultiplayer;
            IEnumerator particleToggle = MainCharacter.runParticle.ParticleToggle(1.5f);
            StartCoroutine(particleToggle);
            GameManager.AnswerCorrect = true;
            GameManager.Force = 500;
            KeyboardController.Instance.SetInputFieldTextColor(true);
        }
        else
        {
            // caller.AnswerImage.SetActive(true);
            GameManager.AnswerCorrect = false;
            GameManager.Force = Random.Range(100f, 250f);
            KeyboardController.Instance.SetInputFieldTextColor(false);
        }


        // caller.GetComponent<Button>().interactable = false;
        MainCharacter.NormalRunActive();
        MainCharacter.ReturnAnimator();
        MainCharacter.playerState = PlayerState.Move;
        MainCharacter.GameCameraSetter();
        MainCharacter.GoNextStep();
        StartCoroutine(DelayAndPlay(percentage));
        MainCharacter.CurtainDestroyer();

        await Task.Delay(500);
        Vector3 playerPos =
            Camera.main.WorldToScreenPoint(MainCharacter.transform.position + MainCharacter.transform.up * 2);
        CanvasManager.instance.AnimateMoneys(playerPos, CanvasManager.instance.totalMoneyImage.position, true,
            letterCount);
    }

    private IEnumerator DelayAndPlay(float percentage)
    {
        yield return new WaitForSeconds(0.7f);
        MainCharacter.AnswerCloser();

        if (percentage > 50)
        {
            MainCharacter.gameObject.transform.DOScale(Vector3.one * 1.3f, 0.5f);
        }
    }

    public float SetSakilDelayTime()
    {
        float time = _sakilDelay * (1 - (percentageValue * 0.01f));
        return time;
    }
}