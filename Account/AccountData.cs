using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AccountData : Singleton<AccountData>
{
    string token;
    UserModel userModel;
    Device device;
    [SerializeField] UrlSO urlSO;
    public UserModel UserModel { get => userModel; }
    public string Token { get => token; }
    public Device Device { get => device; }
    public UrlSO UrlSO { get => urlSO; }

    protected override void Awake()
    {
        base.Awake();
        Helper.GameHelper.Urls = urlSO;
        device = new Device();
        StartCoroutine(GetTokenRequest());
    }
    public void SetUserModel(UserModel userModel)
    {
        this.userModel = userModel;
    }
    IEnumerator GetTokenRequest()
    {
        token = string.Empty;
        using (UnityWebRequest rq = UnityWebRequest.Get(urlSO.getTokenUrl))
        {
            yield return rq.SendWebRequest();
            if (rq.result == UnityWebRequest.Result.Success)
            {
                token = rq.downloadHandler.text.Substring(1, 32);
            }
            else
            {
                HelperUI.Instance.ShowErrorUI("Error: " + rq.error);
                yield return Helper.GameHelper.WaitForSecondsRealtime(5);
                StartCoroutine(GetTokenRequest());
            }
        }
    }
    public void Reset()
    {
        userModel = null;
        device.Reset();
    }
}
