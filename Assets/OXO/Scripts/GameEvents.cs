using UnityEngine;

public class GameEvents : MonoBehaviour
{
    //All events for SDK or etc...

    private void OnEnable()
    {
        Actions.OnGameStarted += OnGameStarted;
        Actions.OnGameCompleted += OnGameCompleted;
        Actions.OnGameFailed += OnGameFailed;
    }


    private void OnDisable()
    {
        Actions.OnGameStarted -= OnGameStarted;
        Actions.OnGameCompleted -= OnGameCompleted;
        Actions.OnGameFailed -= OnGameFailed;
    }

    private void OnGameStarted()
    {

    }
    private void OnGameCompleted()
    {
        
    }
    private void OnGameFailed()
    {
        
    }


}
