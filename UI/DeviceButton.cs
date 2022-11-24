using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceButton : MonoBehaviour
{
    int deviceID;
    [SerializeField] SceneSO controllerScene;
    public int DeviceID { get => deviceID; }

    public void SetDeviceID(int id)
    {
        deviceID = id;
    }
    public void LoadControllerScene()
    {
        bool isLoaded = false;
        HelperUI.Instance.ShowLoadingUI(() => isLoaded == true);
        controllerScene.LoadSceneWithLoadedAction(UnityEngine.SceneManagement.LoadSceneMode.Single, () => isLoaded = true);
    }
    public void SetCurrentDevice()
    {
        AccountData.Instance.Device.CurrentDeviceID = deviceID;
    }
}
