using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Events;

public class SignIn : MonoBehaviour
{
    [SerializeField] TMP_InputField emailIpf;
    [SerializeField] TMP_InputField passwordIpf;
    [SerializeField] Toggle rememberStateToogle;
    [SerializeField] UnityEvent loginEvent;
    bool isLoadSuccess = false;
    private void OnEnable()
    {
        SignInSetting setting = SaveLoadData.Load<SignInSetting>("setting", TypeSaveMothod.Binary);
        if (setting != null)
        {
            rememberStateToogle.isOn = setting.isRemember;
            emailIpf.text = setting.email;
            passwordIpf.text = setting.password;
        }
    }
    public void SignInButton()
    {
        StartCoroutine(GetUserRequest());
    }
    IEnumerator GetUserRequest()
    {
        isLoadSuccess = false;
        HelperUI.Instance.ShowLoadingUI(() => isLoadSuccess == true);
        UserLoginModel model = new UserLoginModel(emailIpf.text, passwordIpf.text);
        string json = JsonConvert.SerializeObject(model);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        using (UnityWebRequest rq = UnityWebRequest.Post(Helper.GameHelper.Urls.signInUrl, "POST"))
        {
            rq.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            rq.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            rq.SetRequestHeader("Content-Type", "application/json");
            yield return rq.SendWebRequest();
            if (rq.result == UnityWebRequest.Result.Success)
            {
                UserModel userModel = JsonConvert.DeserializeObject<UserModel>(rq.downloadHandler.text);
                if (userModel != null)
                {
                    AccountData.Instance.SetUserModel(userModel);
                    StartCoroutine(GetListDeviceRequest());
                }
            }
            else
            {
                if (rq.responseCode == 404)
                {
                    HelperUI.Instance.ShowErrorUI("You've entered an incorrect email address or password.");
                }
                isLoadSuccess = true;
            }
        }
    }
    IEnumerator GetListDeviceRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("userKey", AccountData.Instance.UserModel.UserKey);
        using (UnityWebRequest rq = UnityWebRequest.Get(Helper.GameHelper.Urls.getDeviceUrl))
        {
            rq.uploadHandler = (UploadHandler)new UploadHandlerRaw(form.data);
            rq.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            foreach (var item in form.headers)
            {
                rq.SetRequestHeader(item.Key, item.Value);
            }
            yield return rq.SendWebRequest();
            if (rq.result == UnityWebRequest.Result.Success)
            {
                IEnumerable<DeviceModel> listDevice = JsonConvert.DeserializeObject<List<DeviceModel>>(rq.downloadHandler.text);
                if (listDevice != null)
                {
                    AccountData.Instance.Device.GetListDevice(listDevice);
                }
                isLoadSuccess = true;
                loginEvent?.Invoke();
            }
            else
            {
                HelperUI.Instance.ShowErrorUI("Error: " + rq.error);
            }
        }
    }
    private void OnDisable()
    {
        if (rememberStateToogle.isOn)
        {
            SignInSetting setting = new SignInSetting(rememberStateToogle.isOn, emailIpf.text, passwordIpf.text);
            SaveLoadData.Save<SignInSetting>(setting, "setting", TypeSaveMothod.Binary);
        }
        else
        {
            SaveLoadData.DeleteDataFile("setting");
        }
    }
}
[System.Serializable]
public class SignInSetting
{
    public bool isRemember;
    public string email;
    public string password;

    public SignInSetting(bool isRemember, string email, string password)
    {
        this.isRemember = isRemember;
        this.email = email;
        this.password = password;
    }
}
