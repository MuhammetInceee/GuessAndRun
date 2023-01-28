using System.Collections.Generic;
using MuhammetInce.DesignPattern.Singleton;
using UnityEngine;
using Random = UnityEngine.Random;


public class WallSpawnManagerCPI : LazySingleton<WallSpawnManagerCPI>
{
    private GameObject[] _folderObjets;
    
    [SerializeField] private GameObject[] brickSpawnPoints;
    [SerializeField] private List<GameObject> walls;
    public List<string> tempList;

    [Header("Wall Materials")] 
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material orangeMaterial;

    private void Start()
    {
        _folderObjets = Resources.LoadAll<GameObject>("Cpi");

        foreach (GameObject gO in _folderObjets)
        {
            walls.Add(gO);
        }

        SpawnPointFiller();
        Spawner();
    }


    private void Spawner()
    {
        foreach (GameObject gO in brickSpawnPoints)
        {
            GameObject wall = walls[GetRandomInteger(0, walls.Count)];
            GameObject newObj = Instantiate(wall, gO.transform.position, Quaternion.identity, gO.transform);
            SetWallMaterial(newObj);
        }
    }
    
    public int GetRandomInteger(int minValue, int maxValue)
    {
        return Random.Range(minValue, maxValue);
    }

    private void SetWallMaterial(GameObject go)
    {
        float X = go.transform.parent.transform.localPosition.x;
        WallBangBang bang = go.GetComponent<WallBangBang>();

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
}
