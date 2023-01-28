using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(TabChangeStyle))]
public class TabGroup : MonoBehaviour
{
    [Header("Settings")]

    public int defualtOpenIndex;
    
    [Space]
    public TabButton selectedTab;
    [Space]
    public List<TabButton> tabButtons;

    [Tooltip("Idle -> Hover -> Selected")]
    public List<TabStatus> tabStatuses;
    public TabChangeStyle tabChangeStyle;

    public List<GameObject> objectsToSwap;
    private void Awake()
    {
        tabChangeStyle = GetComponent<TabChangeStyle>();
        StartCoroutine(OrderBySiblingIndex());
    }
    public void Subscribe(TabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }

        tabButtons.Add(button);
    }
    //Hover on the button
    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            UpdateStatus(button, 1);
        }

    }
    public void OnTabSelected(TabButton button)
    {
        if (selectedTab != null)
        {
            selectedTab.DeSelect();
        }

        selectedTab = button;

        ResetTabs();
        UpdateStatus(button, 2);

        selectedTab.Select();

        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            if (i == index)
            {
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }

    }
    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }
    public void ResetTabs()
    {
        foreach (TabButton button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab) { continue; }
            UpdateStatus(button, 0);
        }
    }
    public IEnumerator OrderBySiblingIndex()
    {
        yield return new WaitForFixedUpdate();
        tabButtons = tabButtons.OrderBy(x => x.transform.GetSiblingIndex()).ToList();
    }
    public void UpdateStatus(TabButton button, int status)
    {
        button.background.sprite = tabChangeStyle.tabStatuses[status].sprite;
        button.background.color = tabChangeStyle.tabStatuses[status].color;

        button.UpdateStatus(button, status);
    }
    public IEnumerator OpenButton(int index)
    {
        yield return new WaitForFixedUpdate();
        OnTabSelected(tabButtons[index]);
    }
}
