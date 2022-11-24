using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json;
using System;
using UnityEngine.UI;
namespace Helper
{
    public static class GameHelper
    {
        public static UrlSO Urls { get; set; }
        private static Color pressButtonColor = new Color(0.736f, 0.736f, 0.736f, 1);
        static Dictionary<float, WaitForSecondsRealtime> waitRealTime = new Dictionary<float, WaitForSecondsRealtime>();

        public static Color PressButtonColor { get => pressButtonColor; }

        public static WaitForSecondsRealtime WaitForSecondsRealtime(float time)
        {
            if (waitRealTime.ContainsKey(time))
            {
                return waitRealTime[time];
            }
            WaitForSecondsRealtime wait = new WaitForSecondsRealtime(time);
            waitRealTime.Add(time, wait);
            return wait;
        }
        public static void Reset()
        {
            waitRealTime.Clear();
        }
    }
    public static class AccountHelper
    {
        static string updateUrl = Helper.GameHelper.Urls.infoUpdateUrl;
        public static IEnumerator UpdateRequest(string typeUpdate, string data, string userKey, System.Action onDataChanged = null, System.Action<UnityWebRequest> onError = null)
        {
            string url = $"{updateUrl}/{typeUpdate}";
            WWWForm form = new WWWForm();
            form.AddField("data", data);
            form.AddField("userKey", userKey);
            using (UnityWebRequest rq = UnityWebRequest.Put(url, "PUT"))
            {
                rq.uploadHandler = (UploadHandler)new UploadHandlerRaw(form.data);
                foreach (var item in form.headers)
                {
                    rq.SetRequestHeader(item.Key, item.Value);
                }
                yield return rq.SendWebRequest();
                switch (rq.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        HelperUI.Instance.ShowErrorUI("Error: " + rq.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        {
                            onError?.Invoke(rq);
                        }
                        break;
                    case UnityWebRequest.Result.Success:
                        onDataChanged?.Invoke();
                        break;
                }
            }
        }
    }
    public static class DeviceConfigHelper
    {
        public static TMP_Dropdown deviceConfigList;
        public static Toggle isRecord;
        public static TMP_InputField nameDeviceConfig;
        public static TMP_InputField warnValueDeviceConfig;
        public static TMP_InputField stopValueDeviceConfig;
        public static void ResetAllValue()
        {
            nameDeviceConfig.text = string.Empty;
            warnValueDeviceConfig.text = string.Empty;
            stopValueDeviceConfig.text = string.Empty;
        }
        public static DeviceConfig GetDeviceConfig()
        {
            DeviceConfig deviceConfig = new DeviceConfig();
            deviceConfig.TypeVibration = nameDeviceConfig.text;
            deviceConfig.IsRecord = isRecord.isOn;
            deviceConfig.WarnValue = float.Parse(warnValueDeviceConfig.text);
            deviceConfig.StopValue = float.Parse(stopValueDeviceConfig.text);
            return deviceConfig;
        }
    }
}
