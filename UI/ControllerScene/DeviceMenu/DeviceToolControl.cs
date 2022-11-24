using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
public class DeviceToolControl : MonoBehaviour
{
    [SerializeField] TMP_InputField newName;
    [SerializeField] TextMeshProUGUI deviceName;
    [SerializeField] GameObject renameSuccessfullyBox;
    [SerializeField] SceneSO mainScene;
    public void RemoveDevice()
    {
        StartCoroutine(RemoveDeviceRequest());
    }
    public void RenameDevice()
    {
        CheckValueValid checkFill = new CheckForFillRequiredField(newName.text);
        if (!checkFill.IsValid())
        {
            checkFill.ShowErrorMessage();
            return;
        }
        StartCoroutine(RenameDeviceRequest());
    }
    IEnumerator RemoveDeviceRequest()
    {
        string url = $"{Helper.GameHelper.Urls.removeDeviceUrl}?deviceKey={AccountData.Instance.Device.CurrentDevice?.DeviceModel.DeviceKey}";
        using (UnityWebRequest rq = UnityWebRequest.Delete(url))
        {
            yield return rq.SendWebRequest();
            if (rq.result == UnityWebRequest.Result.Success)
            {
                mainScene.LoadScene(UnityEngine.SceneManagement.LoadSceneMode.Single);
                AccountData.Instance.Device.RemoveCurrentDevice();
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
    IEnumerator RenameDeviceRequest()
    {
        string url = $"{Helper.GameHelper.Urls.renameDeviceUrl}/{AccountData.Instance.Device.CurrentDevice?.DeviceModel.DeviceKey}";
        WWWForm form = new WWWForm();
        form.AddField("deviceName", newName.text);
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
                deviceName.text = $"Device name:\n{newName.text}";
                AccountData.Instance.Device.CurrentDevice.DeviceModel.DeviceName = newName.text;
                renameSuccessfullyBox.SetActive(true);
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
