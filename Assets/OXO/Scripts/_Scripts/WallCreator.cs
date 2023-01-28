#if UNITY_EDITOR
using Sirenix.OdinInspector;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class WallCreator : MonoBehaviour
{
    [HorizontalGroup("Split",width:210)]  
    [PreviewField,HideLabel, Space(25)]
    public Sprite changeImage;
    
    [Header("About Properties of the Object to be Created: "), Space]
    public string trueAnswer;
    public string firstWrongAnswer;
    public string secondWrongAnswer;

    
    
    [Header("About Prefab: "), Space]
    public GameObject wallPrefab;
    public string FolderName;
    

    [Button]
    public void Change()
    {
        SpriteChanger();
        AnswerChanger();
    }

    [Button]
    public void SetAsPrefab()
    {
        string locationPath = "Assets/Resources/" + FolderName + "/" + trueAnswer + " Wall.prefab";

        locationPath = AssetDatabase.GenerateUniqueAssetPath(locationPath);

        PrefabUtility.SaveAsPrefabAsset(wallPrefab, locationPath);
    }

    private void SpriteChanger()
    {
        wallPrefab.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = changeImage;
        wallPrefab.name = trueAnswer;
    }

    private void AnswerChanger()
    {
        //True Answer
        wallPrefab.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = trueAnswer;
        wallPrefab.GetComponent<WallBangBang>().wallName = trueAnswer;
        //WrongAnswers
        wallPrefab.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = firstWrongAnswer;
        wallPrefab.transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = secondWrongAnswer;
    }
}
#endif
