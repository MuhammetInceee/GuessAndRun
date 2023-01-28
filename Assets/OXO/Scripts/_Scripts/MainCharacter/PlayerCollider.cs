using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crown;
using DG.Tweening;
using RootMotion.FinalIK;
using Sirenix.Utilities;
using UnityEngine;
using Wall.Answer;

public class PlayerCollider : MonoBehaviour
{
    private MainCharacter _mainCharacter;
    private TutorialSettings _tutorialSettings;
    private GameObject targetObj;
    private RaycastHit _hit;
    [SerializeField] private LayerMask layerMask;
    public List<GameObject> AIList;

    private ManagerGame GameManager => ManagerGame.Instance;
    private SkinnedMeshRenderer Curtain
    {
        get => _mainCharacter.curtain;
        set => _mainCharacter.curtain = value;
    }
    
    private Transform LeftHandTarget
    {
        get => _mainCharacter.IK.solver.leftHandEffector.target;
        set => _mainCharacter.IK.solver.leftHandEffector.target = value;
    }
    private Transform RightHandTarget
    {
        get => _mainCharacter.IK.solver.rightHandEffector.target;
        set => _mainCharacter.IK.solver.rightHandEffector.target = value;
    }
    
    private void Start()
    {
        _mainCharacter = GetComponent<MainCharacter>();
        _tutorialSettings = GetComponent<TutorialSettings>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WallEnter"))
        {
            WallEnterStopper(other);
            if (Physics.Raycast(transform.position + Vector3.up, Vector3.forward, out _hit, Mathf.Infinity, layerMask.value))
            {
                _mainCharacter.trueAnswer = other.GetComponent<TrueAnswerHolder>().trueAnswer;
                
                
                Curtain = _hit.collider.GetComponent<SkinnedMeshRenderer>();
                PackValueHolder holder = Curtain.GetComponent<PackValueHolder>();
                
                _mainCharacter.SetCurtian(Curtain);
                _mainCharacter.SetPackValueHolder(holder);
                
                LeftHandTarget = holder.LeftHandPosition;
                RightHandTarget = holder.RightHandPosition;
                _mainCharacter.Animator.enabled = false;
                _mainCharacter.IK.enabled = true;
                
                _mainCharacter.OnNewState(PlayerState.Curtain);
                StartCoroutine(TimeToAnswer());
            }
        }
        
        if (other.CompareTag("Wall"))
        {
            other.transform.GetComponent<WallBangBang>().BangBang(GameManager.Force);
            _mainCharacter.TempValueReset();
            if (GameManager.AnswerCorrect)
            {
                DOTween.To(() => _mainCharacter.runSpeed, m => _mainCharacter.runSpeed = m,
                    _mainCharacter.runSpeed / GameManager.PlayerRunMultiplayer, 0.2f);
                CinemachineShake.Instance.ShakeCamera(3, 0.25f);
            }
            else
            {
                transform.DOKill();
                IEnumerator particleToggle = _mainCharacter.stunParticle.ParticleToggle(1);
                StartCoroutine(particleToggle);
                _mainCharacter.Animator.SetBool(_mainCharacter.Fail, true);
                transform.DOMoveZ(transform.position.z + 2, GameManager.FailAnimLength).SetEase(Ease.Linear);


                StartCoroutine(ContinueMove(GameManager.FailAnimLength));
            }

            _mainCharacter.curtain = null;
        }

        if (other.CompareTag("Punch"))
        {
            if (!ManagerGame.Instance.AnswerCorrect) return;
            StartCoroutine(_mainCharacter.Animator.AnimatorToggle(_mainCharacter.Punch,
                ManagerGame.Instance.PunchAnimLength));        }

        if (other.CompareTag("LevelEnd"))
        {
            CrownController.Instance.isCheckEnabled = false;
            _mainCharacter.NormalRunActive();
            _mainCharacter.LevelEndCameraSetter();
            PlayerLevelEndMover();
        }

        if (other.CompareTag("FlipArea"))
        {
            transform.DOKill();
            _mainCharacter.Animator.SetBool(_mainCharacter.CanFlip, true);
            transform.DOLocalJump(new Vector3(0, 7,0), 1, 1, GameManager.FlipAnimLength);
            transform.DOLocalRotate(new Vector3(0, 180, 0), GameManager.FlipAnimLength).OnComplete(() =>
            {
                if (transform.parent.name == "ThirdPlace")
                {
                    _mainCharacter.Animator.SetBool(_mainCharacter.Sad, true);
                }
                else
                {
                    _mainCharacter.Animator.SetBool(_mainCharacter.Happy, true);
                }
                AISetter();
                UIManager.Instance.LevelEndVisualizer(transform.parent.gameObject.name);
            });
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            transform.DOScale(Vector3.one, 0.8f).SetEase(Ease.Linear);
        }
    }

    private IEnumerator TimeToAnswer()
    {
        yield return new WaitForSeconds(0.3f);
        _mainCharacter.LineEntry();
        StartCoroutine(TutorialStart());
    }
    private IEnumerator TutorialStart()
    {
        yield return new WaitForSeconds(1);
        //_tutorialSettings.LineEnter();
    }

    private IEnumerator ContinueMove(float delay)
    {
        yield return new WaitForSeconds(delay);
        _mainCharacter.ContinueRoad(AnswerManager.Instance.SetSakilDelayTime());
        _mainCharacter.Animator.SetBool(_mainCharacter.Fail, false);
        _mainCharacter.SakilRunActive();
    }

    private GameObject WhichAIOn()
    {
        if (AIList.Count == 0) return null;

        if (AIList.Count == 1)
        {
            return AIList[0];
        }
        else
        {
            GameObject on = null;

            for (int i = 1; i < AIList.Count; i++)
            {
                if (AIList[i].transform.position.z > AIList[i - 1].transform.position.z)
                {
                    on = AIList[i];
                }
                else
                {
                    on = AIList[i - 1];
                }
            }
            return on;
        }
    }

    private void PlayerLevelEndMover()
    {
        targetObj = GameManager.CheckTargetPodium();

        transform.DOKill();
        if(transform.parent != null) return;
        transform.parent = targetObj.transform;
            
        transform.LookAt(targetObj.transform);

        transform.DOLocalMoveZ(-0.4f, GameManager.LevelEndRunSpeed).SetSpeedBased();
    }

    private async void AISetter()
    {
        await Task.Delay(250);
        
        GameObject on = WhichAIOn();
        if(on == null) return;

        on.transform.DOKill();
        on.GetComponent<Animator>().enabled = true;
        on.GetComponent<FullBodyBipedIK>().enabled = false;
        on.GetComponent<AIPlayer>().enabled = false;
        on.GetComponent<BoxCollider>().enabled = false;
        on.GetComponent<Animator>().SetBool("happyTime", true);
        on.transform.parent = GameManager.CheckTargetPodium().transform;
        on.transform.localPosition = Vector3.zero;
        on.transform.eulerAngles = new Vector3(0, 180, 0);
        on.transform.DOScale(Vector3.one, 0.01f);
        on.transform.DOLocalMoveY(7.1f, 0.1f).OnComplete(() =>
        {
            on.transform.DOKill();
        }); 
        on.GetComponent<Animator>()
            .SetBool(on.transform.parent.name == "ThirdPlace" ? "sadTime" : "happyTime", true);
        AIList.Remove(on);

        if(AIList.Count == 0) return;
        
        await Task.Delay(50);
        GameObject anotherAI = AIList[0];
        anotherAI.transform.DOKill();
        anotherAI.GetComponent<Animator>().enabled = true;
        anotherAI.GetComponent<FullBodyBipedIK>().enabled = false;
        anotherAI.GetComponent<AIPlayer>().enabled = false;
        anotherAI.GetComponent<BoxCollider>().enabled = false;
        anotherAI.transform.parent = GameManager.CheckTargetPodium().transform;
        anotherAI.transform.localPosition = Vector3.zero;
        anotherAI.transform.eulerAngles = new Vector3(0, 180, 0);
        anotherAI.transform.DOScale(Vector3.one, 0.01f);
        anotherAI.transform.DOLocalMoveY(7.1f, 0.1f).OnComplete(() =>
        {
            anotherAI.transform.DOKill();
        });
        
        anotherAI.GetComponent<Animator>()
            .SetBool(anotherAI.transform.parent.name == "ThirdPlace" ? "sadTime" : "happyTime", true);
    }

    private  void WallEnterStopper(Collider other)
    {
        transform.DOKill();
        transform.DOMoveZ(other.GetComponent<BoxCollider>().bounds.center.z, 0.1f);
    }
}
