using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NativeWebSocket;
using System.Threading.Tasks;
using System;
public class AppWSClient : Singleton<AppWSClient>
{
    Dictionary<string, WebSocket> wss = new Dictionary<string, WebSocket>();
    public WebSocket CurrentSocket
    {
        get
        {
            if (wss.Count >= 0)
            {
                Debug.Log((Helper.DeviceConfigHelper.deviceConfigList.CurrentOptionData() as DeviceConfigOption)?.name);
                return wss[(Helper.DeviceConfigHelper.deviceConfigList.CurrentOptionData() as DeviceConfigOption)?.name];
            }
            return null;
        }
    }
    public async void AddWebsocket(string typeVibration)
    {
        if (wss.ContainsKey(typeVibration))
        {
            return;
        }
        string url = $"{AccountData.Instance.UrlSO.appWsUrl}?deviceKey={AccountData.Instance.Device.CurrentDevice.DeviceModel.DeviceKey}&typeVibration={typeVibration}&token={AccountData.Instance.Token}";
        Debug.Log(url);
        WebSocket ws = new WebSocket(url);
        ws.OnOpen += () =>
        {
            Debug.Log($"{typeVibration} Ws connection open!");
        };

        ws.OnError += (e) =>
        {
            Debug.LogError("Error! " + e);
        };

        ws.OnClose += (e) =>
        {
            Debug.Log($"{typeVibration} Ws connection closed!");
        };
        wss.Add(typeVibration, ws);
        await ws.Connect();
    }
    public void UpdateWebsocket(string oldTypeVibration, string newTypeVibration)
    {
        if (!wss.ContainsKey(oldTypeVibration))
        {
            return;
        }
        RemoveWebsocket(oldTypeVibration);
        AddWebsocket(newTypeVibration);
    }
    public void RemoveWebsocket(string typeVibration)
    {
        if (!wss.ContainsKey(typeVibration))
        {
            return;
        }
        WebSocket ws = wss[typeVibration];
        ws.Close();
        wss.Remove(typeVibration);
    }
    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        foreach (WebSocket ws in wss.Values)
        {
            if (ws != null && ws.State == WebSocketState.Open)
            {
                ws.DispatchMessageQueue();
            }
        }
#endif
    }
    // public async void Disconnect(string typeVibration)
    // {
    //     if (!wss.ContainsKey(typeVibration))
    //     {
    //         return;
    //     }
    //     await wss[typeVibration].Close();
    // }
    public async void DisconnectAll()
    {
        foreach (WebSocket ws in wss.Values)
        {
            if (ws.State == WebSocketState.Open)
            {
                await ws.Close();
            }
        }
        wss.Clear();
    }
    protected override void OnDestroy()
    {
        DisconnectAll();
        base.OnDestroy();
    }
}
