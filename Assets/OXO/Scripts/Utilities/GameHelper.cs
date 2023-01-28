using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
public class GameHelper 
{
    [MenuItem("[Game HELPER]/Reset All Game Data %t")]
    static void ClearData() => PlayerPrefs.DeleteAll();


    [MenuItem("[Game HELPER]/Ping Camera")]
    static void PingCamera()
    {
        Object _object = GameObject.FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
        Selection.activeObject = _object;

        EditorGUIUtility.PingObject(_object);
    }
    [MenuItem("[Game HELPER]/Select Null %g")]
    static void SelectNull() => Selection.activeObject = null;

}
#endif