using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct AppModeElement
{
    public GameObject button;
    public GameObject menu;
    public GameObject tools;
}
public class ModeSelection : MonoBehaviour
{
    [SerializeField] int interactableModeStartIndex;
    [Tooltip("Index 0 always would be default menu")]
    [SerializeField] AppModeElement[] appModeElements;
    public void SelectMode(int modeID)
    {
        for (int i = interactableModeStartIndex; i < appModeElements.Length; i++)
        {
            if (modeID == i)
            {
                appModeElements[i].button.TrySetActive(true);
                appModeElements[i].menu.TrySetActive(true);
                appModeElements[i].tools.TrySetActive(true);
            }
            else
            {
                appModeElements[i].button.TrySetActive(false);
                appModeElements[i].menu.TrySetActive(false);
                appModeElements[i].tools.TrySetActive(false);
            }
        }
    }
}
public static class SetActiveExtention
{
    public static void TrySetActive(this GameObject gameObject, bool state)
    {
        if (gameObject != null)
        {
            gameObject.SetActive(state);
        }
    }
}