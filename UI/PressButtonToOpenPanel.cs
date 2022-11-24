using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PressButtonToOpenPanel : OpenPanel
{
    Image thisImg;
    private void Start()
    {
        thisImg = GetComponent<Image>();
    }
    public override void PressButton()
    {
        if (!isPressed)
        {
            panel.TrySetActive(true);
            thisImg.color = Helper.GameHelper.PressButtonColor;
        }
        else
        {
            panel.TrySetActive(false);
            thisImg.color = Color.white;
        }
        isPressed = !isPressed;
    }
    public override void ShowPanel()
    {
        if (!isPressed)
        {
            panel.TrySetActive(true);
            thisImg.color = Helper.GameHelper.PressButtonColor;
            isPressed = true;
        }
    }
    public override void HidePanel()
    {
        if (isPressed)
        {
            panel.TrySetActive(false);
            thisImg.color = Color.white;
            isPressed = false;
        }
    }

}
