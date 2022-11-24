using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.UI;
public class DeviceConfigPanel : MonoBehaviour
{
    [SerializeField] GameObject addOrUpdateDeviceConfigPanel;
    [SerializeField] GameObject addButton;
    [SerializeField] GameObject updateButton;
    [SerializeField] protected TMP_Dropdown deviceConfigList;
    [SerializeField] protected TMP_InputField nameDeviceConfig;
    [SerializeField] Toggle isRecord;
    [SerializeField] protected TMP_InputField warnValueDeviceConfig;
    [SerializeField] protected TMP_InputField stopValueDeviceConfig;
    private void Awake()
    {
        Helper.DeviceConfigHelper.deviceConfigList = deviceConfigList;
        Helper.DeviceConfigHelper.nameDeviceConfig = nameDeviceConfig;
        Helper.DeviceConfigHelper.isRecord = isRecord;
        Helper.DeviceConfigHelper.warnValueDeviceConfig = warnValueDeviceConfig;
        Helper.DeviceConfigHelper.stopValueDeviceConfig = stopValueDeviceConfig;
    }
    private void OnDisable()
    {
        addOrUpdateDeviceConfigPanel.SetActive(false);
        addButton.SetActive(false);
        updateButton.SetActive(false);
    }
    private void OnDestroy()
    {
        Helper.DeviceConfigHelper.deviceConfigList = null;
        Helper.DeviceConfigHelper.nameDeviceConfig = null;
        Helper.DeviceConfigHelper.isRecord = null;
        Helper.DeviceConfigHelper.warnValueDeviceConfig = null;
        Helper.DeviceConfigHelper.stopValueDeviceConfig = null;
    }
    private void Start()
    {
        StartCoroutine(GetDeviceConfigRequest());
    }
    IEnumerator GetDeviceConfigRequest()
    {
        string url = $"{Helper.GameHelper.Urls.getDeviceConfigUrl}/{AccountData.Instance.Device.CurrentDevice.DeviceModel.DeviceKey}";
        using (UnityWebRequest rq = UnityWebRequest.Get(url))
        {
            rq.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            yield return rq.SendWebRequest();
            if (rq.result == UnityWebRequest.Result.Success)
            {
                IEnumerable<DeviceConfig> deviceConfigs = JsonConvert.DeserializeObject<List<DeviceConfig>>(rq.downloadHandler.text);
                if (deviceConfigs != null)
                {
                    AccountData.Instance.Device.SetDeviceConfig(deviceConfigs);
                }
                foreach (DeviceConfig config in deviceConfigs)
                {
                    deviceConfigList.options.Add(new DeviceConfigOption(config.TypeVibration, config.IsRecord, config.WarnValue, config.StopValue));
                    AppWSClient.Instance.AddWebsocket(config.TypeVibration);
                }
                deviceConfigList.RefreshShownValue();
            }
            else
            {
                HelperUI.Instance.ShowErrorUI("Error: " + rq.error);
            }
        }
    }
}
