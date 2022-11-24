using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class LoggerUI : MonoBehaviour
{
    bool isAutoScroll = true;
    bool isLog = false;
    #region Timer Tool
    [SerializeField] TMP_InputField timerIpf;
    float timeDelay = 0;
    WaitForSecondsRealtime wait;
    #endregion
    private string colorText = "#000000";
    [SerializeField] private ScrollRect uiScrollRect;
    [SerializeField] TMP_Text contentText;
    [SerializeField] TextMeshProUGUI content;
    [SerializeField] RectTransform contentRectTrans;
    private void Start()
    {
        StartCoroutine(Logging());
    }
    private void OnEnable()
    {
        Application.logMessageReceived += LogCallback;
    }
    IEnumerator Logging()
    {
        while (true)
        {
            if (timeDelay == 0)
            {
                yield return null;
            }
            else
            {
                if (wait != null)
                {
                    yield return wait;
                }
            }
            if (isLog)
            {
            }
        }
    }
    private void LogCallback(string message, string stackTrace, LogType type)
    {
        if (type == LogType.Log)
        {
            content.text += $"<sprite=0><color={colorText}> {message}</color>\n\n";
            if (contentRectTrans.sizeDelta.y < content.bounds.size.y)
            {
                contentRectTrans.sizeDelta = new Vector2(contentRectTrans.sizeDelta.x, content.bounds.size.y);
            }
            if (uiScrollRect != null)
            {
                if (isAutoScroll)
                {
                    uiScrollRect.verticalNormalizedPosition = 0f;
                }
            }
        }
    }
    private void OnDisable()
    {
        Application.logMessageReceived -= LogCallback;
    }
    public void SetLoggerState(bool state)
    {
        isLog = state;
    }
    public void SetAutoScroll(bool state)
    {
        isAutoScroll = state;
    }
    public void SetTimer()
    {
        if (float.TryParse(timerIpf.text, out timeDelay))
        {
            if (timeDelay <= 0)
            {
                timeDelay = 0;
            }
            else
            {
                wait = Helper.GameHelper.WaitForSecondsRealtime(timeDelay);
                timerIpf.text = string.Empty;
                if (isAutoScroll)
                {
                    uiScrollRect.verticalNormalizedPosition = 0f;
                }
            }
        }
    }
}
