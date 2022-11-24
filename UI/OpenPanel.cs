using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPanel : MonoBehaviour
{
    [SerializeField] protected GameObject panel;
    [SerializeField] protected bool defaultState;
    protected bool isPressed;
    private void OnDisable()
    {
        if (defaultState)
        {
            ShowPanel();
        }
        else
        {
            HidePanel();
        }
    }
    public virtual void PressButton()
    {
        if (!isPressed)
        {
            panel.TrySetActive(true);
        }
        else
        {
            panel.TrySetActive(false);
        }
        isPressed = !isPressed;
    }
    public virtual void ShowPanel()
    {
        if (!isPressed)
        {
            panel.TrySetActive(true);
            isPressed = true;
        }
    }
    public virtual void HidePanel()
    {
        if (isPressed)
        {
            panel.TrySetActive(false);
            isPressed = false;
        }
    }
}
