using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using Newtonsoft.Json;
public class UpdateDeviceConfig : MonoBehaviour
{
    string currentTypeVibration;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] PressButtonToOpenPanel openUpdatePanel;
    [SerializeField] OpenPanel openUpdateButton;
    [SerializeField] UpdateDeviceConfigButton updateDeviceConfigButton;
    int currentItem;
    int idItem;
    public void EditThisDeviceConfig()
    {
        currentTypeVibration = text.text;
        int length = Helper.DeviceConfigHelper.deviceConfigList.options.Count;
        for (int i = 0; i < length; i++)
        {
            if (Helper.DeviceConfigHelper.deviceConfigList.options[i].text.Equals(text.text))
            {
                if (i != 0)
                {
                    idItem = i;
                    openUpdatePanel.ShowPanel();
                    openUpdateButton.ShowPanel();
                    DeviceConfigOption option = Helper.DeviceConfigHelper.deviceConfigList.options[i] as DeviceConfigOption;
                    Helper.DeviceConfigHelper.nameDeviceConfig.text = option.name;
                    Helper.DeviceConfigHelper.warnValueDeviceConfig.text = option.warnValue.ToString();
                    Helper.DeviceConfigHelper.stopValueDeviceConfig.text = option.stopValue.ToString();
                    Helper.DeviceConfigHelper.deviceConfigList.value = i;
                    updateDeviceConfigButton.SetData(this, openUpdatePanel, openUpdateButton);
                }
                return;
            }
        }
    }
    public void UpdateThisDeviceConfig()
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
        StartCoroutine(UpdateDeviceConfigRequest());
    }
    public IEnumerator UpdateDeviceConfigRequest()
    {
        string url = $"{Helper.GameHelper.Urls.updateDeviceConfigUrl}/{AccountData.Instance.Device.CurrentDevice.DeviceModel.DeviceKey}/{currentTypeVibration}";
        DeviceConfig deviceConfig = Helper.DeviceConfigHelper.GetDeviceConfig();
        string json = JsonConvert.SerializeObject(deviceConfig);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        using (UnityWebRequest rq = UnityWebRequest.Put(url, "PUT"))
        {
            rq.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            rq.SetRequestHeader("Content-Type", "application/json");
            yield return rq.SendWebRequest();
            if (rq.result == UnityWebRequest.Result.Success)
            {
                AccountData.Instance.Device.UpdateDeviceConfig(currentTypeVibration, deviceConfig);
                DeviceConfigOption config = Helper.DeviceConfigHelper.deviceConfigList.options[idItem] as DeviceConfigOption;
                if (config != null)
                {
                    text.text = deviceConfig.TypeVibration;
                    config.text = deviceConfig.TypeVibration;
                    config.name = deviceConfig.TypeVibration;
                    config.isRecord = deviceConfig.IsRecord;
                    config.warnValue = deviceConfig.WarnValue;
                    config.stopValue = deviceConfig.StopValue;
                    AppWSClient.Instance.UpdateWebsocket(currentTypeVibration, deviceConfig.TypeVibration);
                }
                Helper.DeviceConfigHelper.deviceConfigList.value = currentItem;
                Helper.DeviceConfigHelper.deviceConfigList.Hide();
                Helper.DeviceConfigHelper.deviceConfigList.Show();
            }
            else
            {
                HelperUI.Instance.ShowErrorUI("Error: " + rq.error);
            }
        }
    }
}
