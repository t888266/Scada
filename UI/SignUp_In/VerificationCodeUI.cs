using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using UnityEngine.Networking;
public class VerificationCodeUI : MonoBehaviour
{
    [SerializeField] TMP_InputField codeIpf;
    [SerializeField] Button submitButton;
    [SerializeField] GameObject createdPanel;
    SignUp signUp;
    private void Awake()
    {
        signUp = GetComponentInParent<SignUp>();
    }
    IEnumerator SignUpRequest()
    {
        submitButton.interactable = false;
        UserModel model = new UserModel(signUp.EmailIpf.text, signUp.PasswordIpf.text, signUp.UsernameIpf.text, AccountData.Instance.Token);
        string json = JsonConvert.SerializeObject(model);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        string url = $"{Helper.GameHelper.Urls.signUpUrl}/{codeIpf.text}";
        using (UnityWebRequest rq = UnityWebRequest.Post(url, "POST"))
        {
            rq.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            rq.SetRequestHeader("Content-Type", "application/json");
            var sendOp = rq.SendWebRequest();
            HelperUI.Instance.ShowLoadingUI(() => sendOp.isDone == true);
            yield return sendOp;
            switch (rq.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    {
                        HelperUI.Instance.ShowErrorUI("Error: " + rq.error);
                        break;
                    }
                case UnityWebRequest.Result.ProtocolError:
                    {
                        if (rq.responseCode == 400)
                        {
                            HelperUI.Instance.ShowErrorUI("The verification code you have been entered is invalid or expired!");
                        }
                        break;
                    }
                case UnityWebRequest.Result.Success:
                    {
                        createdPanel.SetActive(true);
                        gameObject.SetActive(false);
                        break;
                    }
            }
            submitButton.interactable = true;
        }
    }
    public void SubmitAndsSignUp()
    {
        CheckValueValid checkCode = new CheckCodeValid(codeIpf.text);
        if (!checkCode.IsValid())
        {
            checkCode.ShowErrorMessage();
            return;
        }
        StartCoroutine(SignUpRequest());
    }
    private void OnDisable()
    {
        codeIpf.text = string.Empty;
    }
}
