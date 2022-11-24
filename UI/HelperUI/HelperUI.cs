using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class HelperUI : Singleton<HelperUI>
{
    [SerializeField] TextMeshProUGUI errorText;
    [SerializeField] RectTransform errorBGRectTrans;
    [SerializeField] RectTransform errorTextRectTrans;
    [SerializeField] GameObject loading;
    public void ShowErrorUI(string errorMessage, float timeShow = 1.5f)
    {
        StartCoroutine(ShowError(errorMessage, timeShow));
    }
    public void ShowLoadingUI(Func<bool> condition)
    {
        StartCoroutine(ShowLoading(condition));
    }
    public void ShowLoadingUI(float time)
    {
        StartCoroutine(ShowLoading(time));
    }
    IEnumerator ShowError(string errorMessage, float timeShow)
    {
        errorText.text = $"<sprite=2> {errorMessage}";
        errorBGRectTrans.gameObject.SetActive(true);
        if (errorBGRectTrans.sizeDelta.y < errorText.bounds.size.y)
        {
            errorTextRectTrans.sizeDelta = new Vector2(errorTextRectTrans.sizeDelta.x, errorText.bounds.size.y);
            errorBGRectTrans.sizeDelta = new Vector2(errorBGRectTrans.sizeDelta.x, errorTextRectTrans.sizeDelta.y + 100);
        }
        yield return Helper.GameHelper.WaitForSecondsRealtime(timeShow);
        errorBGRectTrans.gameObject.SetActive(false);
    }
    IEnumerator ShowLoading(Func<bool> condition)
    {
        loading.SetActive(true);
        yield return new WaitUntil(condition);
        loading.SetActive(false);
    }
    IEnumerator ShowLoading(float time)
    {
        loading.SetActive(true);
        yield return Helper.GameHelper.WaitForSecondsRealtime(time);
        loading.SetActive(false);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        Helper.GameHelper.Reset();
    }
}
