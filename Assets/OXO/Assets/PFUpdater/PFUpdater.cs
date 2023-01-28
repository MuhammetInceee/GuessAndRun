using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
namespace PFUpdater
{
    public class PFUpdater : MonoBehaviour
    {
        [HideInInspector]
        public PlayerPrefsType playerPrefsType;

        [HideInInspector]
        public string stringKey, stringValue, stringGet;

        [HideInInspector]
        public string intKey;
        [HideInInspector]
        public int intValue, getInt;

        [HideInInspector]
        public string floatKey;
        [HideInInspector]
        public float floatValue, getFloat;
      
        public void Go()
        {
            Debug.Log(stringKey);
        }
        public void Get()
        {
            if (playerPrefsType == PlayerPrefsType.String)
            {
                stringGet = PlayerPrefs.GetString(stringKey);
            }
            if (playerPrefsType == PlayerPrefsType.Int)
            {
                getInt = PlayerPrefs.GetInt(intKey);
            }
            if (playerPrefsType == PlayerPrefsType.Float)
            {
                getFloat = PlayerPrefs.GetFloat(floatKey);
            }
        }
        public void Set()
        {
            if (playerPrefsType == PlayerPrefsType.String)
            {
                PlayerPrefs.SetString(stringKey, stringValue);
                stringGet = PlayerPrefs.GetString(stringKey);
                Debug.Log($"<color=white><b>{stringGet}</b> </color>");

            }
            if (playerPrefsType == PlayerPrefsType.Int)
            {
                PlayerPrefs.SetInt(intKey, intValue);
                getInt = PlayerPrefs.GetInt(intKey);
            }
            if (playerPrefsType == PlayerPrefsType.Float)
            {
                PlayerPrefs.SetFloat(floatKey, floatValue);
                getFloat = PlayerPrefs.GetFloat(floatKey);
            }
        }

    }
}