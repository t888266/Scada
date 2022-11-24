using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Text.RegularExpressions;
public class SignUp : MonoBehaviour
{
    #region Inputfield
    [SerializeField] TMP_InputField usernameIpf;
    [SerializeField] TMP_InputField emailIpf;
    [SerializeField] TMP_InputField passwordIpf;
    #endregion
    [SerializeField] GameObject verifyCodePanel;

    public TMP_InputField UsernameIpf { get => usernameIpf; }
    public TMP_InputField PasswordIpf { get => passwordIpf; }
    public TMP_InputField EmailIpf { get => emailIpf; }

    bool IsInfoValid()
    {
        CheckValueValid[] checkForFillRequiredFields = new CheckForFillRequiredField[3];
        checkForFillRequiredFields[0] = new CheckForFillRequiredField(usernameIpf.text);
        checkForFillRequiredFields[1] = new CheckForFillRequiredField(emailIpf.text);
        checkForFillRequiredFields[2] = new CheckForFillRequiredField(passwordIpf.text);
        for (int i = 0; i < 3; i++)
        {
            if (!checkForFillRequiredFields[i].IsValid())
            {
                checkForFillRequiredFields[i].ShowErrorMessage();
                return false;
            }
        }
        CheckValueValid checkPasswordLength = new CheckPasswordValid(passwordIpf.text);
        if (!checkPasswordLength.IsValid())
        {
            checkPasswordLength.ShowErrorMessage();
            return false;
        }
        CheckValueValid checkEmailValid = new CheckEmailValid(emailIpf.text);
        if (!checkEmailValid.IsValid())
        {
            checkEmailValid.ShowErrorMessage();
            return false;
        }
        return true;
    }
    public void SignUpAction()
    {
        if (IsInfoValid())
        {
            GetVerificationCode();
        }
    }
    public void GetVerificationCode()
    {
        StartCoroutine(GetVerificationCodeRequest());
    }
    IEnumerator GetVerificationCodeRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("token", AccountData.Instance.Token);
        form.AddField("username", usernameIpf.text);
        form.AddField("email", emailIpf.text);
        using (UnityWebRequest rq = UnityWebRequest.Post(Helper.GameHelper.Urls.getVerificationCodeForREGUrl, form))
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
                        if (rq.responseCode == 409)
                        {
                            HelperUI.Instance.ShowErrorUI("The email has already been taken");
                        }
                    }
                    break;
                case UnityWebRequest.Result.Success:
                    verifyCodePanel.SetActive(true);
                    break;
            }
        }
    }
    private void OnDisable()
    {
        emailIpf.text = string.Empty;
        passwordIpf.text = string.Empty;
        usernameIpf.text = string.Empty;
    }
}

