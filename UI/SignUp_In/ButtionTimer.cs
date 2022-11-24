using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ButtionTimer : MonoBehaviour
{
    Button thisButton;
    TextMeshProUGUI buttonTxt;
    [SerializeField] int seconds;
    int time;
    private void Awake()
    {
        thisButton = GetComponent<Button>();
        buttonTxt = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        StartCoroutine(InterractableAfterSeconds());
    }
    IEnumerator InterractableAfterSeconds()
    {
        time = 0;
        thisButton.interactable = false;
        while (time < seconds)
        {
            time++;
            yield return Helper.GameHelper.WaitForSecondsRealtime(1);
            buttonTxt.text = $"Resend ({seconds - time})";
        }
        buttonTxt.text = "Resend";
        thisButton.interactable = true;
        time = 0;
    }
    public void ReCount()
    {
        StartCoroutine(InterractableAfterSeconds());
    }
}
