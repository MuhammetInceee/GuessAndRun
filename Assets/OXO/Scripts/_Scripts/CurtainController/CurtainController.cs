using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "GuessAndRun/Curtain/Anim Controller")]
public class CurtainController : ScriptableObject
{
    public float MaxCloseSpeed;
    public float CloseSpeed;
    public float CurtainUpPerClick;
    public float CurtainOpenDuration;

    public float PrulleyRotateSpeed = 250f;
    public Ease PrulleyEase = Ease.InQuad;
    
    [Header("For AI: ")] 
    public int MinCurtainOpenValue;
    public int MinPredictValueForTrueAnswer;
    public float RunMultiplier;


    public float AIAnswerMinDelay;
    public float AIAnswerMaxDelay;

}
