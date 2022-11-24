using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Urls", menuName = "SO/UrlSO")]
public class UrlSO : ScriptableObject
{
    public string getTokenUrl;
    [Header("Sign Up Urls")]
    public string getVerificationCodeForREGUrl;
    public string signUpUrl;
    [Header("Sign In Urls")]
    public string signInUrl;
    public string getDeviceUrl;
    public string getVerificationCodeForRSTUrl;
    public string verifyRSTCodeUrl;
    public string infoUpdateUrl;
    [Header("Device Urls")]
    public string addDeviceUrl;
    public string renameDeviceUrl;
    public string removeDeviceUrl;
    public string addDeviceConfigUrl;
    public string changeRecordStateUrl;
    public string getDeviceConfigUrl;
    public string removeDeviceConfigUrl;
    public string updateDeviceConfigUrl;
    [Header("Websocket Urls")]
    public string appWsUrl;
}
