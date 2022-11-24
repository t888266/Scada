using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DeviceConfigOption : TMP_Dropdown.OptionData
{
    public DeviceConfigOption(string text) : base(text) { }

    public DeviceConfigOption() { }

    public DeviceConfigOption(string text, Sprite image) : base(text, image) { }
    public DeviceConfigOption(string name, bool isRecord, float warnValue, float stopValue)
    {
        SetWarnAndStopValue(warnValue, stopValue);
        SetDefaultValue(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
        text = $@"{name}";
        this.name = name;
        this.isRecord = isRecord;
    }
    public string name;
    public bool isRecord;
    public float warnValue;
    public float stopValue;
    public float defaultXAcc;
    public float defaultYAcc;
    public float defaultZAcc;
    public void SetWarnAndStopValue(float warnValue, float stopValue)
    {
        this.warnValue = warnValue;
        this.stopValue = stopValue;
    }
    public void SetDefaultValue(float xAcc, float yAcc, float zAcc)
    {
        defaultXAcc = xAcc;
        defaultYAcc = yAcc;
        defaultZAcc = zAcc;
    }
    public DeviceConfig GetDeviceConfig()
    {
        DeviceConfig config = new DeviceConfig();
        config.IsRecord = isRecord;
        config.TypeVibration = name;
        config.WarnValue = warnValue;
        config.StopValue = stopValue;
        config.DefaultXAcc = defaultXAcc;
        config.DefaultYAcc = defaultYAcc;
        config.DefaultZAcc = defaultZAcc;
        return config;
    }
}