using UnityEngine;
using System.Collections;
using UnityEditor;

namespace PFUpdater
{


    public enum PlayerPrefsType
    {
        String, Float, Int
    }
#if UNITY_EDITOR

    [CustomEditor(typeof(PFUpdater))]
    public class PFUpdaterEditor : Editor
    {
        PlayerPrefsType playerPrefsType;
        public override void OnInspectorGUI()
        {

            PFUpdater myScript = (PFUpdater)target;
            playerPrefsType = (PlayerPrefsType)EditorGUILayout.EnumPopup("PlayerPrefs Type?", myScript.playerPrefsType);

            DrawDefaultInspector();

            PlayerPrefs(myScript.playerPrefsType, myScript);

            EditorGUILayout.Space();

            if (GUILayout.Button("Set"))
            {
                myScript.Set();
                myScript.Get();
            }

        }

        [MenuItem("PF Updater/Create Player Prefs Updater Object")]
        static void Init()
        {
            
            if (FindObjectOfType<PFUpdater>())
            {
                GameObject pfUpdater = FindObjectOfType<PFUpdater>().gameObject;
                Selection.SetActiveObjectWithContext(pfUpdater, pfUpdater);
            }
            else
            {
                GameObject pfUpdaterNew = new GameObject("PlayerPrefsUpdater");
         
                PFUpdater PFUpdaterScript = pfUpdaterNew.AddComponent<PFUpdater>();
                
                Selection.SetActiveObjectWithContext(pfUpdaterNew, pfUpdaterNew);
            }

        }

        void PlayerPrefs(PlayerPrefsType type, PFUpdater myScript)
        {
            GUIStyle s = new GUIStyle(EditorStyles.textField);
            s.normal.textColor = Color.green;
            s.alignment = TextAnchor.MiddleCenter;

            switch (type)
            {
                case PlayerPrefsType.String:
                    myScript.stringKey = EditorGUILayout.TextField("Key", myScript.stringKey);
                    myScript.stringValue = EditorGUILayout.TextField("Value", myScript.stringValue);
                    myScript.playerPrefsType = playerPrefsType;
                    myScript.Get();

                    EditorGUILayout.Space();

                    //if (!string.IsNullOrEmpty(myScript.stringGet))
                    EditorGUILayout.LabelField($"{myScript.stringGet}", s, GUILayout.ExpandWidth(true));

                    break;
                case PlayerPrefsType.Int:
                    myScript.intKey = EditorGUILayout.TextField("Key:", myScript.intKey);
                    myScript.intValue = EditorGUILayout.IntField("Value:", myScript.intValue);
                    myScript.playerPrefsType = playerPrefsType;
                    myScript.Get();

                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField($"{myScript.getInt}", s, GUILayout.ExpandWidth(true));
                    break;

                case PlayerPrefsType.Float:
                    myScript.floatKey = EditorGUILayout.TextField("Key:", myScript.floatKey);
                    myScript.floatValue = EditorGUILayout.FloatField("Value:", myScript.floatValue);
                    myScript.playerPrefsType = playerPrefsType;
                    break;
                default:
                    Debug.LogError("Unrecognized Option");
                    break;
            }
        }
    }
#endif
}