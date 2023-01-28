using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MuhammetInce.DesignPattern.Singleton;
using UnityEngine;
using Random = UnityEngine.Random;


public class WallSpawnManager : LazySingleton<WallSpawnManager>
{
    [SerializeField] private int wallNameLength;
    private GameObject[] _folderObjets;

    [SerializeField] private GameObject[] brickSpawnPoints;
    [SerializeField] private List<GameObject> walls;
    public List<string> tempList;

    [Header("Wall Materials")] [SerializeField]
    private Material redMaterial;

    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material orangeMaterial;
    int currentLevel => LevelManager.instance.level;
    public int maxLevel = 4;

    private void Start()
    {
        tempList = ES3.Load("list", tempList);
        int currentLevel = LevelManager.instance.level;


        if (currentLevel > maxLevel)
        {
            GetWalls(1, 2, 3, 4);
        }
        else
        {
            GetLevelsWall(currentLevel);
        }

        SpawnPointFiller();
        StartCoroutine(Spawner());
    }

    private List<GameObject> GetLevelsWall(int currentLevel)
    {
        walls = Resources.LoadAll<GameObject>("Level" + currentLevel).ToList();
        return walls;
    }

    private void GetWalls(params int[] levels)
    {
        List<GameObject> levelsGameObjects = new List<GameObject>();
        foreach (var level in levels)
        {
            List<GameObject> tempWall = GetLevelsWall(level).ToList();
            foreach (var wall in tempWall)
            {
                levelsGameObjects.Add(wall);
            }
        }

        walls = levelsGameObjects.ToList();
    }

    private IEnumerator Spawner()
    {
        for (int i = 0; i < brickSpawnPoints.Length; i++)
        {
            if (walls.Count <= tempList.Count)
            {
                tempList.Clear();
            }

            GameObject wall = walls[GetRandomInteger(0, walls.Count)];

            if (!tempList.Contains(wall.name))
            {
                GameObject newGO = Instantiate(wall, brickSpawnPoints[i].transform.position, Quaternion.identity,
                    brickSpawnPoints[i].transform);
                SetWallMaterial(newGO, wall.name);
                tempList.Add(wall.name);
            }
            else
            {
                GameObject newPrefab = walls[GetRandomInteger(0, walls.Count)];
                GameObject again = Instantiate(newPrefab,
                    brickSpawnPoints[i].transform.position, Quaternion.identity, brickSpawnPoints[i].transform);
                while (tempList.Contains(again.name))
                {
                    Destroy(again.gameObject);
                    again = Instantiate(walls[GetRandomInteger(0, walls.Count)],
                        brickSpawnPoints[i].transform.position, Quaternion.identity, brickSpawnPoints[i].transform);

                    yield return new WaitForFixedUpdate();
                }

                tempList.Add(again.name);
                SetWallMaterial(again, newPrefab.name);
            }
        }

        yield break;
    }

    public int GetRandomInteger(int minValue, int maxValue)
    {
        return Random.Range(minValue, maxValue);
    }

    private void SetWallMaterial(GameObject go, string prefabName)
    {
        float X = go.transform.parent.transform.localPosition.x;
        WallBangBang bang = go.GetComponent<WallBangBang>();

        bang.wallName = prefabName.Substring(0, prefabName.Length - 5).ToLower();
        switch (X)
        {
            case > 3:
                bang.MaterialChanger(orangeMaterial);
                break;
            case < -3:
                bang.MaterialChanger(redMaterial);
                break;
            case < 3 and > -3:
                bang.MaterialChanger(greenMaterial);
                break;
        }
    }

    private void SpawnPointFiller()
    {
        brickSpawnPoints = GameObject.FindGameObjectsWithTag("Spawn");
    }

    private void OnApplicationQuit()
    {
        ES3.Save("list", tempList);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            ES3.Save("list", tempList);
        }
    }
}