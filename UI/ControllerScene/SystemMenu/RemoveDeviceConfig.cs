using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
public class RemoveDeviceConfig : MonoBehaviour
{
    [SerializeField] TMP_Dropdown deviceConfigList;
    [SerializeField] TextMeshProUGUI text;
    public void RemoveThisDeviceConfig()
    {
        int length = deviceConfigList.options.Count;
        for (int i = 0; i < length; i++)
        {
            if (deviceConfigList.options[i].text.Equals(text.text))
            {
                if (i != 0)
                {
                    deviceConfigList.value = 0;
                    deviceConfigList.options.RemoveAt(i);
                    deviceConfigList.RefreshShownValue();
                    deviceConfigList.Hide();
                }
                return;
            }
        }
    }
    IEnumerator RemoveThisDeviceConfigRequest()
    {
        string url = $"{Helper.GameHelper.Urls.removeDeviceConfigUrl}?deviceKey={AccountData.Instance.Device.CurrentDevice?.DeviceModel.DeviceKey}&typeVibration={text.text}";
        using (UnityWebRequest rq = UnityWebRequest.Delete(url))
        {
            yield return rq.SendWebRequest();
            if (rq.result == UnityWebRequest.Result.Success)
            {
                AccountData.Instance.Device.RemoveCurrentDevice();
                AppWSClient.Instance.RemoveWebsocket(text.text);
            }
            else
            {
                if (rq.responseCode == 400)
                {
                    HelperUI.Instance.ShowErrorUI("An unexpected error occurred!");
                }
                else
                {
                    HelperUI.Instance.ShowErrorUI("Error: " + rq.error);
                }
            }
        }
    }
}
