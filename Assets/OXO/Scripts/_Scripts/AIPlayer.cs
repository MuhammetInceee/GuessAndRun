using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crown;
using DG.Tweening;
using RootMotion;
using RootMotion.FinalIK;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIPlayer : MonoBehaviour
{
    private FullBodyBipedIK _ik;
    private Animator _animator;
    private RaycastHit _hit;
    private bool _curtainCanClose = true;
    private bool _canMove = true;
    private bool _trueAnswer;
    private bool _firstClick = true;

    private bool isWaitingDelay = false;
    private int _winChance;
    private float _predictValue;
    private float _force;

    [SerializeField] private float _runSpeed;

    [Header("Animator String Hashes")] private static readonly int CanRun = Animator.StringToHash("canRun");
    private static readonly int Punch = Animator.StringToHash("canPunch");
    private static readonly int Fail = Animator.StringToHash("fail");
    private static readonly int Happy = Animator.StringToHash("happyTime");
    private static readonly int CanSakil = Animator.StringToHash("canSakil");

    [Header("About Particles: "), Space] [SerializeField]
    private ParticleSystem stunParticle;

    [SerializeField] private ParticleSystem runParticle;

    [Header("About AI Movement: "), Space] [SerializeField]
    private int currentPath;

    [SerializeField] private List<GameObject> path;

    [Header("About Curtain: "), Space] [SerializeField]
    private CurtainController curtainController;

    [SerializeField] private SkinnedMeshRenderer curtain;
    [SerializeField] private LayerMask layerMask;


    private ManagerGame GameManager => ManagerGame.Instance;
    private GameObject WallSprite => curtain.transform.parent.GetChild(1).GetChild(0).GetChild(0).gameObject;

    private Transform LeftHandTarget
    {
        get => _ik.solver.leftHandEffector.target;
        set => _ik.solver.leftHandEffector.target = value;
    }

    private PlayerCollider PlayerCollider => MainCharacter.Instance.GetComponent<PlayerCollider>();

    private Transform RightHandTarget
    {
        get => _ik.solver.rightHandEffector.target;
        set => _ik.solver.rightHandEffector.target = value;
    }

    private float CurtainHeight
    {
        get => curtain.GetBlendShapeWeight(0);
        set => curtain.SetBlendShapeWeight(0, value);
    }

    private float GetRandomFloat(float minValue, float maxValue)
    {
        return Random.Range(minValue, maxValue);
    }

    private int GetRandomInt(int minValue, int maxValue)
    {
        return Random.Range(minValue, maxValue);
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _runSpeed = GameManager.RunSpeed;
        _ik = GetComponent<FullBodyBipedIK>();
        _ik.enabled = false;
    }

    private void Update()
    {
        runParticle.transform.position = transform.position;
        Click();
    }

    private void Click()
    {
        if (!_firstClick) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (!CanvasManager.instance.IsPointerOverUIObject())
            {
                _animator.SetBool(CanRun, true);
                float z = path[currentPath].GetComponent<Collider>().bounds.center.z;
                transform.DOMoveZ(z, _runSpeed).SetSpeedBased().SetEase(Ease.Linear);
                _firstClick = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WallEnter"))
        {
            WallEnterStopper(other);
            _predictValue = GetRandomInt(0, curtainController.MinCurtainOpenValue);

            if (Physics.Raycast(transform.position + Vector3.up, Vector3.forward, out _hit, Mathf.Infinity,
                    layerMask.value))
            {
                curtain = _hit.collider.GetComponent<SkinnedMeshRenderer>();
                Transform prulley = curtain.GetComponent<PackValueHolder>().Prulley.transform;
                
                OnCurtainState(prulley);
                PackValueHolder holder = curtain.GetComponent<PackValueHolder>();
                LeftHandTarget = holder.LeftHandPosition;
                RightHandTarget = holder.RightHandPosition;
                _animator.enabled = false;
                _ik.enabled = true;
                StartCoroutine(CurtainOpener());
            }
        }

        if (other.CompareTag("Wall"))
        {
            curtain = null;
            _canMove = true;
            other.transform.GetComponent<WallBangBang>().BangBang(_force);

            if (_trueAnswer)
            {
                DOTween.To(() => _runSpeed, m => _runSpeed = m,
                    _runSpeed / curtainController.RunMultiplier, 0.2f);
            }
            else
            {
                transform.DOKill();
                StartCoroutine(stunParticle.ParticleToggle(1));
                _animator.SetBool(Fail, true);

                transform.DOMoveZ(transform.position.z + 2, GameManager.FailAnimLength).SetEase(Ease.Linear);
                StartCoroutine(ContinueMove(GameManager.FailAnimLength));
            }
        }

        if (other.CompareTag("FlipArea"))
        {
            transform.DOKill();
            _animator.SetBool("canFlip", true);
            transform.DOLocalJump(new Vector3(0, 7, 0), 1, 1, GameManager.FlipAnimLength);
            transform.DOLocalRotate(new Vector3(0, 180, 0), GameManager.FlipAnimLength).OnComplete(() =>
            {
                _animator.SetBool(Happy, true);
            });
        }

        if (other.CompareTag("LevelEnd"))
        {
            CrownController.Instance.isCheckEnabled = false;
            if (transform.parent != null) return;
            transform.DOKill();
            if (PlayerCollider.AIList.Contains(gameObject))
            {
                PlayerCollider.AIList.Remove(gameObject);
            }

            GameObject targetObj = GameManager.CheckTargetPodium();
            transform.parent = targetObj.transform;

            transform.LookAt(targetObj.transform);
            transform.DOLocalMoveZ(0, GameManager.LevelEndRunSpeed).SetSpeedBased();
        }

        if (other.CompareTag("Punch"))
        {
            if (!_trueAnswer) return;
            StartCoroutine(_animator.AnimatorToggle(Punch, ManagerGame.Instance.PunchAnimLength));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            transform.DOScale(Vector3.one, 0.8f).SetEase(Ease.Linear);
        }
    }

    private IEnumerator CurtainOpener()
    {
        yield break;
        
        if (transform.parent != null) yield break;
        yield return new WaitForSeconds(GetRandomFloat(0, 0.05f));
        if (curtain == null) yield break;
        float endValue = Mathf.Clamp( CurtainHeight - curtainController.CurtainUpPerClick,0,85);
        
        Transform pulley = curtain.GetComponent<PackValueHolder>().Prulley.transform;
        
        DOTween.To(() => CurtainHeight, m => CurtainHeight = m, endValue,
                0.4f)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                if (CurtainHeight > _predictValue)
                {
                    pulley.Rotate(-Vector3.right * curtainController.PrulleyRotateSpeed);
                }
            }).OnComplete(() =>
            {
                if (!_curtainCanClose) return;

                StartCoroutine(CurtainOpener());
            });

        if (CurtainHeight <= _predictValue)
        {
            DOVirtual.DelayedCall(Random.Range(curtainController.AIAnswerMinDelay, curtainController.AIAnswerMaxDelay),
            AISetAnswer);
            // AISetAnswer();
            _curtainCanClose = true;
        }
    }

    private void AISetAnswer()
    {
        if (!_canMove) return;
        _ik.enabled = false;
        _animator.enabled = true;

        _winChance = GetRandomInt(0, 3);

        if (_predictValue <= curtainController.MinPredictValueForTrueAnswer)
        {
            if (_winChance <= 1)
            {
                TrueAnswerCase();
            }
            else
            {
                WrongAnswerCase();
            }
        }
        else
        {
            if (_winChance > 1)
            {
                TrueAnswerCase();
            }
            else
            {
                WrongAnswerCase();
            }
        }

        CurtainDestroyer();
        NextPath();
        NormalRunActive();
        _canMove = false;
    }

    private void TrueAnswerCase()
    {
        _trueAnswer = true;
        _runSpeed *= curtainController.RunMultiplier;
        StartCoroutine(runParticle.ParticleToggle(1.5f));
        StartCoroutine(AIScaler());
        _force = 500;
    }

    private void WrongAnswerCase()
    {
        _trueAnswer = false;
        _force = GetRandomFloat(100, 250);
    }

    private void CurtainDestroyer()
    {
        DOTween.To(() => CurtainHeight, m => CurtainHeight = m, -10, 0.5f)
            .SetEase(Ease.OutQuad).OnComplete(() =>
            {
                curtain.gameObject.SetActive(false);
                WallSprite.SetActive(false);
            });
    }

    private void NextPath()
    {
        currentPath++;
        float z = path[currentPath].GetComponent<Collider>().bounds.center.z;
        transform.DOMoveZ(z, _runSpeed).SetSpeedBased().SetEase(Ease.Linear);
    }

    private void ContinueRoad()
    {
        transform.DOMoveZ(path[currentPath].transform.position.z, GameManager.AfterSakilSpeed).SetEase(Ease.Linear)
            .SetSpeedBased();
        SakilRunActive();
    }

    private IEnumerator ContinueMove(float delay)
    {
        yield return new WaitForSeconds(delay);
        ContinueRoad();
        _animator.SetBool(Fail, false);
    }

    private IEnumerator AIScaler()
    {
        yield return new WaitForSeconds(0.5f);

        transform.DOScale(Vector3.one * 1.3f, 0.5f);
    }

    private void WallEnterStopper(Collider other)
    {
        transform.DOKill();
        transform.DOMoveZ(other.GetComponent<BoxCollider>().bounds.center.z, 0.1f);
    }

    public void NormalRunActive()
    {
        _animator.SetBool(CanSakil, false);
        _animator.SetBool(CanRun, true);
    }

    public void SakilRunActive()
    {
        _animator.SetBool(CanSakil, true);
        _animator.SetBool(CanRun, false);
    }
    
    public void OnCurtainState(Transform Prulley)
    {
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
            curtainController.CurtainOpenDuration)
            .SetRelative()
            .OnComplete(()=>StartCoroutine(OnCurtainOpened()));

        #endregion
    }

    private IEnumerator OnCurtainOpened()
    {
        yield return new WaitForSeconds(Random.Range(curtainController.AIAnswerMinDelay,
            curtainController.AIAnswerMaxDelay));
        AISetAnswer();
    }
}