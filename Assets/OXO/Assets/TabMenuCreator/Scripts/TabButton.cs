using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public TabGroup tabGroup;
    public Image background;
    public UnityEvent onTabSelected; 
    public UnityEvent onTabDeSelected;

    public bool useSpecialStatus;
    private TabChangeStyle tabChangeStyle;

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(gameObject.name);
        tabGroup.OnTabEnter(this);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnTabExit(this);
    }

    private void Start()
    {
        background = GetComponent<Image>();
        tabChangeStyle = GetComponent<TabChangeStyle>();
        tabGroup.Subscribe(this);
    }
    public void Select()
    {
        onTabSelected?.Invoke();
    }
    
    public void DeSelect()
    {
        onTabDeSelected?.Invoke();
    }

    public void UpdateStatus(TabButton button, int status)
    {
        if (!tabChangeStyle) { return; }

        button.background.sprite = tabChangeStyle.tabStatuses[status].sprite;
        button.background.color = tabChangeStyle.tabStatuses[status].color;
    }


}