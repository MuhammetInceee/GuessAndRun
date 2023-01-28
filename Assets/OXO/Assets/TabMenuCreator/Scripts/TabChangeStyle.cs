using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TabChangeStyle : MonoBehaviour
{
    public List<TabStatus> tabStatuses;
    
}
[System.Serializable]
public class TabStatus
{
    public string StatusName;
    public Sprite sprite;
    public Color color = Color.black;
}