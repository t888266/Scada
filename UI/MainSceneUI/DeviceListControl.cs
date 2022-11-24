using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class DeviceListControl : MonoBehaviour
{
    [SerializeField] GameObject deviceButton;
    [SerializeField] TMP_InputField nameIpf;
    private void Awake()
    {
        AccountData.Instance.Device.DeviceData.Sort();
        int id = 0;
        foreach (DeviceData item in AccountData.Instance.Device.DeviceData)
        {
            TextMeshProUGUI txt = Instantiate(deviceButton, Vector3.zero, Quaternion.identity, transform.GetChild(0)).GetComponentInChildren<TextMeshProUGUI>();
            if (txt != null)
            {
                DeviceButton deviceButton = txt.GetComponentInParent<DeviceButton>();
                if (deviceButton != null)
                {
                    deviceButton.SetDeviceID(id++);
                }
                txt.text = item.DeviceModel.DeviceName;
            }
        }
    }
    public void AddNewDevice()
    {
        string name;
        if (nameIpf.text.Equals(string.Empty))
        {
            name = $"Device {AccountData.Instance.Device.DeviceData.Count + 1}";
        }
        else
        {
            if (nameIpf.text.Length <= 14)
            {
                name = nameIpf.text;
            }
            else
            {
                name = $"{nameIpf.text.Substring(0, 11)}...";
            }
        }
        StartCoroutine(AddDeviceRequest(name));
    }
    IEnumerator AddDeviceRequest(string deviceName)
    {
        WWWForm form = new WWWForm();
        form.AddField("userKey", AccountData.Instance.UserModel.UserKey);
        form.AddField("deviceID", AccountData.Instance.Device.DeviceData.Count);
        form.AddField("deviceName", deviceName);
        using (UnityWebRequest rq = UnityWebRequest.Post(Helper.GameHelper.Urls.addDeviceUrl, form))
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
                            HelperUI.Instance.ShowErrorUI("An unexpected error occurred!");
                        }
                    }
                    break;
                case UnityWebRequest.Result.Success:
                    {
                        TextMeshProUGUI txt = Instantiate(deviceButton, Vector3.zero, Quaternion.identity, transform.GetChild(0)).GetComponentInChildren<TextMeshProUGUI>();
                        DeviceModel model = JsonConvert.DeserializeObject<DeviceModel>(rq.downloadHandler.text);
                        if (model != null)
                        {
                            AccountData.Instance.Device.AddDevice(model);
                            if (txt != null)
                            {
                                DeviceButton deviceButton = txt.GetComponentInParent<DeviceButton>();
                                if (deviceButton != null)
                                {
                                    deviceButton.SetDeviceID(model.DeviceID);
                                }
                                txt.text = deviceName;
                            }
                            nameIpf.text = string.Empty;
                        }
                        else
                        {
                            HelperUI.Instance.ShowErrorUI("An unexpected error occurred!");
                        }
                    }
                    break;
            }
        }
    }
}
