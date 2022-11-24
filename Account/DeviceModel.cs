using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeviceModel
{
    public DeviceModel()
    {
    }

    public DeviceModel(int deviceID, string deviceKey, string deviceName)
    {
        DeviceID = deviceID;
        DeviceKey = deviceKey;
        DeviceName = deviceName;
    }

    public int DeviceID { get; set; }
    public string DeviceKey { get; set; }
    public string DeviceName { get; set; }
}
public interface ITypeVibration
{
    public string TypeVibration { get; set; }
}
public class DeviceConfig : ITypeVibration
{
    public string TypeVibration { get; set; }
    public bool IsRecord { get; set; }
    public float WarnValue { get; set; }
    public float StopValue { get; set; }
    public float DefaultXAcc { get; set; }
    public float DefaultYAcc { get; set; }
    public float DefaultZAcc { get; set; }
}
public class DeviceSavedData : ITypeVibration
{
    public string TypeVibration { get; set; }
    public DateTime TimeSaved { get; set; }
    public float Temperature { get; set; }
    public float XAcc { get; set; }
    public float YAcc { get; set; }
    public float ZAcc { get; set; }
}
public class AccelerationData
{
    public float XAcc { get; set; }
    public float YAcc { get; set; }
    public float ZAcc { get; set; }
}
public class DeviceData : IComparable<DeviceData>
{
    public DeviceModel DeviceModel { get; set; }
    public ListByTypeVibration<DeviceConfig> DeviceConfig { get; set; }
    public ListByTypeVibration<DeviceSavedData> Datas { get; set; }
    public DeviceData()
    {
        DeviceConfig = new ListByTypeVibration<DeviceConfig>();
        Datas = new ListByTypeVibration<DeviceSavedData>();
    }
    public int CompareTo(DeviceData other)
    {
        return this.DeviceModel.DeviceID.CompareTo(other.DeviceModel.DeviceID);
    }
}
public class Device
{
    bool isAvailable;
    int currentDeviceID = -1;
    List<DeviceData> deviceData = new List<DeviceData>();

    public List<DeviceData> DeviceData { get => deviceData; }
    public int CurrentDeviceID { get => currentDeviceID; set => currentDeviceID = value; }
    public DeviceData CurrentDevice
    {
        get
        {
            if (currentDeviceID != -1)
            {
                return deviceData[currentDeviceID];
            }
            else
            {
                return null;
            }
        }
    }

    public void AddDevice(DeviceModel model)
    {
        DeviceData data = new DeviceData();
        data.DeviceModel = model;
        deviceData.Add(data);
    }
    public void AddDevice(DeviceData data)
    {
        deviceData.Add(data);
    }
    public void RemoveDevice(int deviceID)
    {
        deviceData.RemoveAt(deviceID);
    }
    public void RemoveCurrentDevice()
    {
        deviceData.RemoveAt(currentDeviceID);
    }
    public void GetListDevice(IEnumerable<DeviceModel> models)
    {
        foreach (var item in models)
        {
            AddDevice(item);
        }
    }
    public void AddDeviceConfig(DeviceConfig config)
    {
        Debug.Log(deviceData[currentDeviceID].DeviceConfig == null);
        deviceData[currentDeviceID].DeviceConfig.Add(config);
    }
    public void SetDeviceConfig(IEnumerable<DeviceConfig> deviceConfigs)
    {
        foreach (DeviceConfig item in deviceConfigs)
        {
            deviceData[currentDeviceID].DeviceConfig.Add(item);
        }
    }
    public void UpdateDeviceConfig(string typeVibration, DeviceConfig config)
    {
        int typeVibrationID = deviceData[currentDeviceID].DeviceConfig.IndexOf(typeVibration);
        if (typeVibrationID != -1)
        {
            deviceData[currentDeviceID].DeviceConfig[typeVibrationID] = config;
        }
    }
    // public IEnumerable<DeviceData> GetDeviceData(int deviceID)
    // {
    //     foreach (DeviceSavedData item in deviceData[deviceID].Data)
    //     {
    //         yield return item;
    //     }
    // }
    public void Reset()
    {
        currentDeviceID = -1;
        deviceData.Clear();
    }
}
public class ListByTypeVibration<T> : List<T> where T : ITypeVibration
{
    public int IndexOf(string typeVibration)
    {
        int index = 0;
        foreach (var item in this)
        {
            if (item.TypeVibration.Equals(typeVibration))
            {
                return index;
            }
            index++;
        }
        return -1;
    }
}
