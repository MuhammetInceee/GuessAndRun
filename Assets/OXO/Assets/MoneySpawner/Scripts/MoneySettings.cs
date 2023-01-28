using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "Money Settings ", menuName = "Oxo/Settings/New Money Settings", order = 1)]
public class MoneySettings : ScriptableObject
{
    [Header("Will spawn prefab")]
    public GameObject moneyPrefab;

    [Header("A point to B point duration")]
    public float duration;

    [Header("Object Pool SpawnAmount")]
    public int spawnAmount;

    [Header("Generate Positions")]
    public Vector3 offset;

    [Header("SpawnSettings")]
    public int horizontalAmount;
    public int verticalAmount;
    public int forwardAmount;

    public bool optimizedSpawn;

    [TextArea, Header("Just Info")]
    public string Info;
    private void OnValidate()
    {
        int totalSpawn = (horizontalAmount * verticalAmount * forwardAmount);

        Info = $"Total pos amount is {totalSpawn}";

        if (totalSpawn > spawnAmount)
            spawnAmount = totalSpawn;
    }


}

