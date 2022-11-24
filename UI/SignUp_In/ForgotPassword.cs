using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
public class ForgotPassword : MonoBehaviour
{
    string userKey;
    [SerializeField] TMP_InputField emailIpf;
    [SerializeField] TMP_InputField codeIpf;
    [SerializeField] TMP_InputField passwordIpf;
    [SerializeField] TMP_InputField confirmPasswordIpf;
    [SerializeField] GameObject getVerificationCodePanel;
    [SerializeField] GameObject verifyCodePanel;
    [SerializeField] GameObject changePasswordPanel;
    [SerializeField] GameObject changePasswordSuccessfullyPanel;
    public void GetVerificationCode()
    {
        CheckValueValid checkMail = new CheckEmailValid(emailIpf.text);
        if (!checkMail.IsValid())
        {
            checkMail.ShowErrorMessage();
            return;
        }
        StartCoroutine(GetVerificationCodeRequest());
    }
    public void VerifyCode()
    {
        CheckValueValid checkCode = new CheckCodeValid(codeIpf.text);
        if (!checkCode.IsValid())
        {
            checkCode.ShowErrorMessage();
            return;
        }
        StartCoroutine(VerifyCodeRequest());
    }
    public void UpdatePassword()
    {
        CheckValueValid checkPassword = new CheckPasswordValid(passwordIpf.text);
        if (!checkPassword.IsValid())
        {
            checkPassword.ShowErrorMessage();
            return;
        }
        if (!passwordIpf.text.Equals(confirmPasswordIpf.text))
        {
            HelperUI.Instance.ShowErrorUI("The new password and confirmation password do not match.");
            return;
        }
        StartCoroutine(Helper.AccountHelper.UpdateRequest("password", passwordIpf.text, userKey, () => changePasswordSuccessfullyPanel.SetActive(true)));
    }
    IEnumerator GetVerificationCodeRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("token", AccountData.Instance.Token);
        form.AddField("email", emailIpf.text);
        using (UnityWebRequest rq = UnityWebRequest.Post(Helper.GameHelper.Urls.getVerificationCodeForRSTUrl, form))
        {
            yield return rq.SendWebRequest();
            switch (rq.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    HelperUI.Instance.ShowErrorUI("Error: " + rq.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    {
                        if (rq.responseCode == 404)
                        {
                            HelperUI.Instance.ShowErrorUI("No account found with the email address.");
                        }
                    }
                    break;
                case UnityWebRequest.Result.Success:
                    verifyCodePanel.SetActive(true);
                    getVerificationCodePanel.SetActive(false);
                    break;
            }
        }
    }
    IEnumerator VerifyCodeRequest()
    {
        string url = $"{Helper.GameHelper.Urls.verifyRSTCodeUrl}/{codeIpf.text}";
        WWWForm form = new WWWForm();
        form.AddField("token", AccountData.Instance.Token);
        form.AddField("email", emailIpf.text);
        using (UnityWebRequest rq = UnityWebRequest.Post(url, form))
        {
            rq.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            yield return rq.SendWebRequest();
            switch (rq.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    HelperUI.Instance.ShowErrorUI("Error: " + rq.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    {
                        if (rq.responseCode == 400)
                        {
                            HelperUI.Instance.ShowErrorUI("The verification code you have been entered is invalid or expired!");
                        }
                    }
                    break;
                case UnityWebRequest.Result.Success:
                    userKey = rq.downloadHandler.text.Substring(1, 16);
                    changePasswordPanel.SetActive(true);
                    verifyCodePanel.SetActive(false);
                    break;
            }
        }
    }
    private void OnDisable()
    {
        userKey = string.Empty;
        emailIpf.text = string.Empty;
        codeIpf.text = string.Empty;
        passwordIpf.text = string.Empty;
        confirmPasswordIpf.text = string.Empty;
        getVerificationCodePanel.SetActive(true);
        verifyCodePanel.SetActive(false);
        changePasswordPanel.SetActive(false);
        changePasswordSuccessfullyPanel.SetActive(false);
    }
}
