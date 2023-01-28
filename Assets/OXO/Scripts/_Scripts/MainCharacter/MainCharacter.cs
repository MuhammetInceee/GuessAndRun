using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinemachine;
using DG.Tweening;
using MuhammetInce.DesignPattern.Singleton;
using RootMotion.FinalIK;
using UnityEngine;
using ElephantSDK;
using Key;

public enum PlayerState
{
    Wait,
    Move,
    Curtain,
    Glory
}

public class MainCharacter : LazySingleton<MainCharacter>
{
    private TutorialSettings _tutorialSettings;

    private readonly float _curtainMaxSize = 70;
    private GameObject _buttonParents;

    private Coroutine _closerRoutine;
    private bool _clickStop;
    private bool _canRotate;
    private bool _canPunch = true;
    private float _tempCloseSpeed;
    private Tween movementTween;

    public FullBodyBipedIK IK;
    public PlayerState playerState;

    [Header("About Player Particle Systems: "), Space]
    public ParticleSystem stunParticle;

    public ParticleSystem runParticle;
    private PackValueHolder packValueHolder;

    [Header("About Camera: "), Space] [SerializeField]
    private GameObject answerCam;

    [SerializeField] private GameObject gameCam;
    [SerializeField] private GameObject levelEndCam;

    [Header("Animation Hash Codes")] [HideInInspector]
    public int CanRun = Animator.StringToHash("canRun");

    [HideInInspector] public int Fail = Animator.StringToHash("fail");
    [HideInInspector] public int Sad = Animator.StringToHash("sadTime");
    [HideInInspector] public int Punch = Animator.StringToHash("canPunch");
    [HideInInspector] public int Happy = Animator.StringToHash("happyTime");
    [HideInInspector] public int CanSakil = Animator.StringToHash("canSakil");
    [HideInInspector] public int CanFlip = Animator.StringToHash("canFlip");

    [Header("About Curtain Anim: "), Space]
    public SkinnedMeshRenderer curtain;

    [SerializeField] private CurtainController curtainController;

    [Header("About Player Move Path: "), Space] [SerializeField]
    private List<GameObject> path;

    [SerializeField] private int currentStep;

    public float runSpeed = 0;


    private GameObject ButtonParent => curtain.transform.parent.GetChild(1).GetChild(0).GetChild(2).gameObject;
    private GameObject WallSprite => curtain.transform.parent.GetChild(1).GetChild(0).GetChild(0).gameObject;

    private float CurtainHeight
    {
        get => curtain.GetBlendShapeWeight(0);
        set => curtain.SetBlendShapeWeight(0, value);
    }

    private bool CurtainMaxSize => CurtainHeight >= _curtainMaxSize;
    private bool CurtainCanClose => curtain != null && _clickStop && CurtainHeight > 0;
    public Animator Animator => GetComponent<Animator>();
    private ManagerGame GameManager => ManagerGame.Instance;

    public string trueAnswer { get; set; }
    private string _tempTrueAnswer;

    private void Start()
    {
        playerState = PlayerState.Wait;
        IK = GetComponent<FullBodyBipedIK>();
        IK.enabled = false;
        _tutorialSettings = GetComponent<TutorialSettings>();
        packValueHolder = GetComponent<PackValueHolder>();
    }

    private void Update()
    {
        FirstClickChecker();

        if (Input.GetMouseButtonDown(1))
            click = !click;

        runParticle.transform.position = transform.position;

        #region About Curtain

        if (playerState != PlayerState.Curtain) return;

        #region Tap Tap Mechanic

        /*
        if (Input.GetMouseButtonDown(0))
            Click();

        if (Input.GetMouseButton(0))
            Hold();


        if (Input.GetMouseButtonUp(0))
            if (_closerRoutine == null)
                _closerRoutine = StartCoroutine(ClickedUp());
     
        CanRotate();
           */

        #endregion

        #region Text Mechanic

        #endregion

        #endregion
    }

    private void FirstClickChecker()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!CanvasManager.instance.IsPointerOverUIObject())
            {
                if (playerState == PlayerState.Wait)
                {
                    runSpeed = GameManager.RunSpeed;
                    Elephant.LevelStarted(LevelManager.instance.level);
                    playerState = PlayerState.Move;
                    float z = path[0].transform.position.z;
                    transform.DOMoveZ(z, runSpeed).SetSpeedBased().SetEase(Ease.Linear);
                    Animator.SetBool(CanRun, true);
                }
            }
        }
    }

    #region About Curtain

    private bool click = true;

    private void Click()
    {
        if (!click)
            return;

        if (CurtainHeight <= 0) return;

        Transform pulley = curtain.GetComponent<PackValueHolder>().Prulley.transform;


        CloseRotateRoutine();


        DOTween.To(() => CurtainHeight, m => CurtainHeight = m, CurtainHeight - curtainController.CurtainUpPerClick,
                0.2f).SetEase(Ease.Linear).OnUpdate(
                () =>
                {
                    _canRotate = false;
                    _clickStop = false;
                    pulley.Rotate(-Vector3.right * GameManager.PlayerPulleyRotateMultiplayer);
                })
            .OnComplete(() =>
            {
                if (CurtainHeight <= GameManager.AnswerCameOut)
                {
                    AnswerVisualizer();
                    //_tutorialSettings.AnswerEnter();
                   // _tutorialSettings.LineCloser();
                }

                if (CurtainHeight <= 0)
                {
                    CurtainHeight = 0;
                }
            });
    }

    private bool holding;


    void Hold()
    {
        if (click)
            return;

        if (!curtain)
            return;

        if (playerState != PlayerState.Curtain) return;

        holding = true;
        _canRotate = false;

        CloseRotateRoutine();

        Transform pulley = curtain.GetComponent<PackValueHolder>().Prulley.transform;


        CurtainHeight -= curtainController.CurtainUpPerClick * 0.04f;
        pulley.Rotate(-Vector3.right * GameManager.PlayerPulleyRotateMultiplayer);

        if (CurtainHeight <= GameManager.AnswerCameOut)
        {
            AnswerVisualizer();
        }

        if (CurtainHeight <= 0)
        {
            CurtainHeight = 0;
        }
    }

    private void CurtainCloser()
    {
        if (holding)
            return;


        if (CurtainCanClose)
        {
            CurtainHeight += _tempCloseSpeed * Time.deltaTime;


            if (_tempCloseSpeed + curtainController.CloseSpeed > curtainController.MaxCloseSpeed)
            {
                _tempCloseSpeed = curtainController.MaxCloseSpeed;
            }
            else
            {
                _tempCloseSpeed += curtainController.CloseSpeed;
            }

            if (CurtainMaxSize)
            {
                CurtainHeight = _curtainMaxSize;
            }
        }
    }

    private void CloseRotateRoutine()
    {
        if (_closerRoutine != null)
        {
            StopCoroutine(_closerRoutine);
            _closerRoutine = null;
        }
    }

    #endregion

    public void AnswerCameraSetter()
    {
        gameCam.SetActive(false);
        answerCam.SetActive(true);
    }

    public void GameCameraSetter()
    {
        answerCam.SetActive(false);
        gameCam.SetActive(true);
    }

    public void LevelEndCameraSetter()
    {
        gameCam.GetComponent<CinemachineVirtualCamera>().Follow = null;
        gameCam.SetActive(false);
        levelEndCam.SetActive(true);
    }

    public void AnswerVisualizer()
    {
        // ButtonParent.SetActive(true);
        // ButtonParent.transform.parent.GetChild(4).gameObject.SetActive(true);
        if (!Equals(_tempTrueAnswer, trueAnswer))
        {
            _tempTrueAnswer = trueAnswer;
            KeyboardController.Instance.OnAnswerCanShow();
        }
    }

    public void AnswerCloser()
    {
        ButtonParent.SetActive(false);
        ButtonParent.transform.parent.GetChild(4).gameObject.SetActive(false);
       // _tutorialSettings.AnswerCloser();
    }

    public void GoNextStep()
    {
        currentStep++;
        float z = path[currentStep].transform.position.z;
        transform.DOLocalMoveZ(z, runSpeed).SetEase(Ease.Linear).SetSpeedBased();
        Animator.SetBool(CanRun, true);
    }

    public void LineEntry()
    {
        Animator.SetBool(CanRun, false);
        AnswerCameraSetter();
    }

    public void CurtainDestroyer()
    {
        DOTween.To(() => CurtainHeight, m => CurtainHeight = m, -10, 0.5f)
            .SetEase(Ease.OutQuad).OnComplete(() =>
            {
                curtain.gameObject.SetActive(false);
                WallSprite.SetActive(false);
                _tempCloseSpeed = 0;
            });
    }

    public async void ContinueRoad(float sakilDelay)
    {
        movementTween = transform.DOMoveZ(path[currentStep].transform.position.z, GameManager.AfterSakilSpeed)
            .SetEase(Ease.Linear)
            .SetSpeedBased();
        await Task.Delay((int)(sakilDelay * 1000));
        movementTween.Kill();

        NormalRunActive();
        transform.DOMoveZ(path[currentStep].transform.position.z, GameManager.RunSpeed).SetEase(Ease.Linear)
            .SetSpeedBased();
    }

    private IEnumerator ClickedUp()
    {
        yield return new WaitForSeconds(1f);

        holding = false;
        _clickStop = true;
        _canRotate = true;

        StartCoroutine(Rotate());
    }

    private IEnumerator Rotate()
    {
        if (holding) yield break;
        _canRotate = true;
    }

    private void CanRotate()
    {
        if (_canRotate && curtain != null && CurtainHeight < _curtainMaxSize && CurtainHeight != 0)
        {
            CurtainCloser();

            PackValueHolder packValueHolder = curtain.GetComponent<PackValueHolder>();

            packValueHolder.Prulley.transform.Rotate(Vector3.right * (GameManager.PlayerPulleyRotateMultiplayer * 2));
        }
    }

    public void ReturnAnimator()
    {
        IK.enabled = false;
        Animator.enabled = true;
    }

    public void TempValueReset()
    {
        _tempCloseSpeed = 0;
    }

    public void NormalRunActive()
    {
        Animator.SetBool(CanSakil, false);
        Animator.SetBool(CanRun, true);
    }

    public void SakilRunActive()
    {
        Animator.SetBool(CanSakil, true);
        Animator.SetBool(CanRun, false);
    }

    public void OnNewState(PlayerState newState)
    {
        playerState = newState;
        switch (newState)
        {
            case PlayerState.Curtain:
                OnCurtainState();
                break;
        }
    }

    public void OnCurtainState()
    {
        #region OpenKeyboard

        _tutorialSettings.AnswerEnter();
        KeyboardController.Instance.OnAnswerCanShow();
        KeyboardController.Instance.GetTrueAnswer(true, trueAnswer);

        #endregion

        #region CurtainBlendShape

        float currentValue = curtain.GetBlendShapeWeight(0);
        DOVirtual.Float(currentValue, 0, curtainController.CurtainOpenDuration, OnUpdate).SetEase(Ease.Linear);

        void OnUpdate(float value)
        {
            curtain.SetBlendShapeWeight(0, value);
        }

        #endregion

        #region PrulleyRotate

        Prulley.DORotate(Vector3.left * curtainController.PrulleyRotateSpeed * 100f,
            curtainController.CurtainOpenDuration).SetRelative().OnComplete(OnCurtainOpened);

        #endregion
    }

    private static readonly int ReactionAnimID = Animator.StringToHash("Reaction");

    public void OnCurtainOpened()
    {
        if (playerState != PlayerState.Curtain)
        {
            return;
        }

        IK.enabled = false;
        Animator.enabled = true;
        Animator.SetTrigger(ReactionAnimID);
        print("OnCurtainOpened");
    }

    public void SetCurtian(SkinnedMeshRenderer _curtain)
    {
        curtain = _curtain;
    }

    private Transform Prulley { get; set; }

    public void SetPackValueHolder(PackValueHolder _packValueHolder)
    {
        packValueHolder = _packValueHolder;
        Prulley = packValueHolder.Prulley.transform;
    }
}