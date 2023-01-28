using UnityEngine;

public class InteractivePosition : MonoBehaviour
{
    [Header("Interactive objects")]
    public Transform playerTrm;
    public Material materialRef;

    private void Awake()
    {
        playerTrm = GameObject.Find("Player").transform;
        materialRef = GetComponent<Renderer>().material;
    }
    private void Update()
    {
        materialRef.SetVector("_PlayerPosition", playerTrm.position);
    }
}
