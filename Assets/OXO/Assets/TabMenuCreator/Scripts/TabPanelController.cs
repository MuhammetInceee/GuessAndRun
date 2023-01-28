using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TabPanelController : MonoBehaviour
{
    public Transform parent;

    public TabGroup tabGroup;
    public int defualtOpenMenuIndex;

    public void OpenTabMenu()
    {
        parent.gameObject.SetActive(true);
        StartCoroutine(tabGroup.OpenButton(defualtOpenMenuIndex));
    }
    public void CloseTabMenu()
    {
        parent.gameObject.SetActive(false);
    }
}
