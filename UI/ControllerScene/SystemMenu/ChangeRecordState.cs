using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class ChangeRecordState : MonoBehaviour
{
    [SerializeField] GameObject stopRecordButton;
    [SerializeField] Toggle recordState;
    public void Change()
    {
        if (Helper.DeviceConfigHelper.deviceConfigList != null)
        {
            int currentTypeVibrationID = Helper.DeviceConfigHelper.deviceConfigList.value;
            if (currentTypeVibrationID != 0)
            {
                StartCoroutine(ChangeRecordStateRequest());
            }

        }
    }
    public void RefreshShownValue()
    {
        int currentTypeVibrationID = Helper.DeviceConfigHelper.deviceConfigList.value;
        if (currentTypeVibrationID != 0)
        {
            recordState.isOn = (Helper.DeviceConfigHelper.deviceConfigList.CurrentOptionData() as DeviceConfigOption).isRecord;
            if (recordState.isOn)
            {
                stopRecordButton.SetActive(true);

            }
            else
            {
                stopRecordButton.SetActive(false);
            }
            return;
        }
        stopRecordButton.SetActive(false);
    }
    IEnumerator ChangeRecordStateRequest()
    {
        DeviceConfigOption option = Helper.DeviceConfigHelper.deviceConfigList.CurrentOptionData() as DeviceConfigOption;
        string currentTypeVibration = option.name;
        string url = $"{Helper.GameHelper.Urls.changeRecordStateUrl}/{AccountData.Instance.Device.CurrentDevice.DeviceModel.DeviceKey}/{currentTypeVibration}";
        WWWForm form = new WWWForm();
        form.AddField("isRecord", recordState.isOn.ToString());
        using (UnityWebRequest rq = UnityWebRequest.Put(url, "PUT"))
        {
            rq.uploadHandler = (UploadHandler)new UploadHandlerRaw(form.data);
            foreach (var item in form.headers)
            {
                rq.SetRequestHeader(item.Key, item.Value);
            }
            yield return rq.SendWebRequest();
            if (rq.result == UnityWebRequest.Result.Success)
            {
                option.isRecord = recordState.isOn;
                AccountData.Instance.Device.UpdateDeviceConfig(currentTypeVibration, option.GetDeviceConfig());
                if (recordState.isOn)
                {
                    stopRecordButton.SetActive(true);
                }
                else
                {
                    stopRecordButton.SetActive(false);
                }
            }
            else
            {
                recordState.isOn = !recordState.isOn;
                HelperUI.Instance.ShowErrorUI("Error: " + rq.error);
            }
        }
    }
}
