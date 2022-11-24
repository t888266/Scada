using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DeviceView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI deviceName;
    [SerializeField] TextMeshProUGUI deviceKey;
    [SerializeField] TextMeshProUGUI typeVibration;
    [SerializeField] TextMeshProUGUI defaultValue;
    private void Start()
    {
        SetData();
        defaultValue.text = $"Default values:\nX-axis acceleration: Unset\nY-axis acceleration: Unset\nZ-axis acceleration: Unset";
        typeVibration.text = "Type vibration:\nNone";
    }
    public void SetData()
    {
        deviceName.text = $"Device name:\n{AccountData.Instance.Device.CurrentDevice.DeviceModel.DeviceName}";
        deviceKey.text = $"Device key:\n{AccountData.Instance.Device.CurrentDevice.DeviceModel.DeviceKey}";
    }
    public void SetChangeableData()
    {
        if (Helper.DeviceConfigHelper.deviceConfigList.value != 0)
        {
            DeviceConfigOption option = Helper.DeviceConfigHelper.deviceConfigList.CurrentOptionData() as DeviceConfigOption;
            typeVibration.text = $"Type vibration:\n{option.name}";
            if (option.defaultXAcc == Mathf.Infinity)
            {
                defaultValue.text = $"Default values:\nX-axis acceleration:Unset\nY-axis acceleration:Unset\nZ-axis acceleration:Unset";
            }
            else
            {

            }
            return;
        }
        typeVibration.text = "Type vibration:\nNone";
        defaultValue.text = $"Default values:\nX-axis acceleration:Unset\nY-axis acceleration:Unset\nZ-axis acceleration:Unset";
    }
    public void CopyKeyToClipboard()
    {
        AccountData.Instance.Device.CurrentDevice.DeviceModel.DeviceKey.CopyToClipboard();
    }
}
public static class Extension
{
    public static void CopyToClipboard(this string str)
    {
        GUIUtility.systemCopyBuffer = str;
    }
    public static TMP_Dropdown.OptionData CurrentOptionData(this TMP_Dropdown dropdown)
    {
        if (dropdown.options.Count > 0)
        {
            return dropdown.options[dropdown.value];
        }
        return null;
    }
}