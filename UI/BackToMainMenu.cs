using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class BackToMainMenu : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] SceneSO mainScene;
    [SerializeField] GameObject hightLightButton;
    bool isOnButton = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isOnButton)
        {
            hightLightButton.SetActive(true);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isOnButton = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOnButton = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isOnButton)
        {
            mainScene.LoadScene(UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
        else
        {
            hightLightButton.SetActive(false);
        }
    }
}
