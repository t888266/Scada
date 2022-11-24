using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using Newtonsoft.Json;
public class AddDeviceConfig : MonoBehaviour
{
    public void AddNewDeviceConfig()
    {
        Helper.DeviceConfigHelper.nameDeviceConfig.text = Helper.DeviceConfigHelper.nameDeviceConfig.text.Trim();
        CheckValueValid[] checkFill = new CheckForFillRequiredField[3];
        checkFill[0] = new CheckForFillRequiredField(Helper.DeviceConfigHelper.nameDeviceConfig.text);
        checkFill[1] = new CheckForFillRequiredField(Helper.DeviceConfigHelper.warnValueDeviceConfig.text);
        checkFill[2] = new CheckForFillRequiredField(Helper.DeviceConfigHelper.stopValueDeviceConfig.text);
        CheckValueValid[] checkNumber = new CheckNumber[2];
        checkNumber[0] = new CheckNumber(Helper.DeviceConfigHelper.warnValueDeviceConfig.text);
        checkNumber[1] = new CheckNumber(Helper.DeviceConfigHelper.stopValueDeviceConfig.text);
        foreach (var item in checkFill)
        {
            if (!item.IsValid())
            {
                item.ShowErrorMessage();
                return;
            }
        }
        foreach (var item in checkNumber)
        {
            if (!item.IsValid())
            {
                item.ShowErrorMessage();
                return;
            }
        }
        StartCoroutine(AddDeviceConfigRequest());
    }
    public IEnumerator AddDeviceConfigRequest()
    {
        string url = $"{Helper.GameHelper.Urls.addDeviceConfigUrl}/{AccountData.Instance.Device.CurrentDevice.DeviceModel.DeviceKey}";
        DeviceConfig deviceConfig = Helper.DeviceConfigHelper.GetDeviceConfig();
        string json = JsonConvert.SerializeObject(deviceConfig);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        using (UnityWebRequest rq = UnityWebRequest.Post(url, "POST"))
        {
            rq.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            rq.SetRequestHeader("Content-Type", "application/json");
            yield return rq.SendWebRequest();
            if (rq.result == UnityWebRequest.Result.Success)
            {
                Helper.DeviceConfigHelper.deviceConfigList.options.Add(new DeviceConfigOption(deviceConfig.TypeVibration, deviceConfig.IsRecord, deviceConfig.WarnValue, deviceConfig.StopValue));
                AccountData.Instance.Device.AddDeviceConfig(deviceConfig);
                Helper.DeviceConfigHelper.deviceConfigList.RefreshShownValue();
                AppWSClient.Instance.AddWebsocket(deviceConfig.TypeVibration);
            }
            else
            {
                HelperUI.Instance.ShowErrorUI("Error: " + rq.error);
            }
        }
    }
}
