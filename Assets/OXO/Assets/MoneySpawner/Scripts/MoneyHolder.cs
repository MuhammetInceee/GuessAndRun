using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class MoneyHolder : MonoBehaviour
{
    public MoneySettings moneySettings;
    public List<Transform> positionPosList;

    private void Start()
    {
        StartCoroutine(GeneratePositions());
    }

    public IEnumerator GeneratePositions()
    {
        float time = Time.time;

        float right = 0;
        float top = 0;
        float forward = 0;

        for (int i = 0; i < moneySettings.forwardAmount; i++)
        {
            GameObject line = new GameObject($"[{i}]Line");
            line.transform.parent = transform;
            //line.transform.localRotation= Quaternion.identity;

            for (int j = 0; j < moneySettings.verticalAmount; j++)
            {
                for (int k = 0; k < moneySettings.horizontalAmount; k++)
                {
                    GameObject pos = new GameObject($"{k}Pos");

                    pos.transform.position = new Vector3(right, top, forward) + transform.position;
                    pos.transform.parent = line.transform;

                    right += moneySettings.offset.x;

                    positionPosList.Add(pos.transform);
                    if(moneySettings.optimizedSpawn)
                        yield return new WaitForEndOfFrame();
                }
                top += moneySettings.offset.y;
                right = 0;
                if (moneySettings.optimizedSpawn)
                    yield return new WaitForEndOfFrame();
            }
            line.transform.localRotation = Quaternion.identity;

            forward += moneySettings.offset.z;
            right = 0;
            top = 0;
        }
        time = Time.time - time;
        if (debugSettings.debugMode)
            Debug.Log($"Created in {time}ms");
    }
    public Transform GetCurrentTransform()
    {
        return positionPosList.Where(x => x.childCount < 1).FirstOrDefault();
    }
    private void OnDrawGizmos()
    {
        if (debugSettings.debugMode == false) { return; }
        foreach (var item in positionPosList)
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = debugSettings.color;
            Gizmos.DrawSphere(item.position, debugSettings.radius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(item.position, debugSettings.radius);

        }
    }
    [Space(5)]
    public DebugSettings debugSettings;

    [System.Serializable]

   
    public class DebugSettings
    {
        [Header("Open Debug Mode")]
        [SerializeField] public bool debugMode;
        [Space]
        [SerializeField] public Color color = Color.yellow;
        [SerializeField] public float radius = 0.06f;

    }

    public void Spawn()
    {
        MoneyManager.instance.Spawn(this);
    }

}
