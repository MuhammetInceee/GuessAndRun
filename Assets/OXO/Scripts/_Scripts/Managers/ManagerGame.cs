using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Linq;
using Controllers.Upgrade;

public class ManagerGame : Singleton<ManagerGame>
{
    [SerializeField] private GameObject tapToPlay;
    
    [Title("Global Datas: ")]
    public bool AnswerCorrect;
    public float Force;
    public float _RunSpeed;
    public float AnswerCameOut;
    public float _AfterSakilSpeed;
    
    [Title("Player Global Datas: ")]
    public float PlayerRunMultiplayer;

    public float PlayerPulleyRotateMultiplayer;


    [Title("All Players: ")] 
    public List<GameObject> AllPlayers;
    public float LevelEndRunSpeed;

    [Title("About Level End: ")] 
    public List<GameObject> PodiumPoses;

    [Title("About Animation Clip: ")]
    public AnimationClip FailAnimClip;
    public AnimationClip PunchClip;
    public AnimationClip FlipClip;
    public float FailAnimLength => FailAnimClip.length;
    public float PunchAnimLength => PunchClip.length;
    public float FlipAnimLength => FlipClip.length;

    public float RunSpeed
    {
        get => PlayerPrefs.GetFloat("runspeed",_RunSpeed);
        set => PlayerPrefs.SetFloat("runspeed", value);
    }
    
    public float AfterSakilSpeed
    {
        get => PlayerPrefs.GetFloat("aftersakilSpeed",_AfterSakilSpeed);
        set => PlayerPrefs.SetFloat("aftersakilSpeed", value);
    }

    private void OnEnable()
    {
        UpgradeController.onSpeedUpgrade += UpgradeSpeed;
    }

    private void OnDisable()
    {
        UpgradeController.onSpeedUpgrade -= UpgradeSpeed;
    }

    public GameObject CheckTargetPodium()
    {
        GameObject target = PodiumPoses.FirstOrDefault(m => m.transform.childCount == 0);
        
        if (target == null)
        {
            Debug.LogError("Podium Poses List Give Null Expression");
            return null;
        }
        else
        {
            return target;
        }
    }

    public void UpgradeSpeed(float value)
    {
        AfterSakilSpeed += value;
        RunSpeed+=value;
    }
}
