using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDefaultValue : MonoBehaviour
{
    List<AccelerationData> datas = new List<AccelerationData>(140);
    public bool IsFinishedGetData => datas.Count == datas.Capacity;
    public void OnReceiveDefaultValue()
    {
        AppWSClient.Instance.CurrentSocket.OnMessage += GetDefaultValues;
    }
    public void OnCancleReceiveDefaultValue()
    {
        AppWSClient.Instance.CurrentSocket.OnMessage += GetDefaultValues;
    }
    void GetDefaultValues(byte[] bytes)
    {
        string mess = System.Text.Encoding.UTF8.GetString(bytes);
        Debug.Log(mess);
    }
}
